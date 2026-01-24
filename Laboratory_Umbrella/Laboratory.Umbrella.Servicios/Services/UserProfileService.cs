using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Services;

public class UserProfileService : BaseService, IUserProfileService
{
    private readonly IRepository<UserProfile> _userProfiles;

    public UserProfileService(IRepository<UserProfile> userProfiles)
    {
        _userProfiles = userProfiles;
    }

    public async Task<MetaDataResponse<List<UserProfileResponse>>> GetByParameters(UserProfileByParameters request)
    {
        Expression<Func<UserProfile, bool>> filter = p =>
            (string.IsNullOrEmpty(request.UserId) || p.UserId == request.UserId) &&
            (string.IsNullOrEmpty(request.ProfileId) || p.ProfileId == request.ProfileId);

        var paged = await _userProfiles.GetPagedAsync(
            request.Page,
            request.PageSize,
            filter,
            p => p.UserId,
            ascending: true);

        var response = paged.Items.Select(p => new UserProfileResponse
        {
            Id = p.Id,
            UserId = p.UserId,
            ProfileId = p.ProfileId
        }).ToList();

        return new(response, BuildMeta(request.Page, request.PageSize, paged.TotalCount));
    }

    public async Task<MetaDataResponse<UserProfileResponse>> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new MetaDataResponse<UserProfileResponse>(null, null);

        var entity = await _userProfiles.GetByIdAsync(id);
        if (entity == null)
            return new MetaDataResponse<UserProfileResponse>(null, null);

        var response = new UserProfileResponse
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProfileId = entity.ProfileId
        };

        return new(response, null);
    }

    public async Task<MetaDataResponse<bool>> Save(SaveUserProfileRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            var entity = new UserProfile
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                ProfileId = request.ProfileId,
                CreatedAt = DateTime.Now
            };

            await _userProfiles.AddAsync(entity);
            return new(true, null);
        }

        var existing = await _userProfiles.GetByIdAsync(request.Id);
        if (existing == null)
            return new(false, null);

        existing.UserId = request.UserId;
        existing.ProfileId = request.ProfileId;
        existing.UpdatedAt = DateTime.Now;

        await _userProfiles.UpdateAsync(existing);
        return new(true, null);
    }

    public async Task<MetaDataResponse<bool>> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new(false, null);

        await _userProfiles.DeleteAsync(id);
        return new(true, null);
    }

    private MetaResponse BuildMeta(int page, int pageSize, long totalCount)
    {
        return new MetaResponse
        {
            TotalCount = (int)totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            HasNextPage = HasNextPage(page, pageSize, totalCount),
            HasPreviousPage = page > 1
        };
    }
}
