using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Services;

public class ProfileService : BaseService, IProfileService
{
    private readonly IRepository<Profile> _profiles;

    public ProfileService(IRepository<Profile> profiles)
    {
        _profiles = profiles;
    }

    public async Task<MetaDataResponse<List<ProfileResponse>>> GetByParameters(ProfileByParameters request)
    {
        Expression<Func<Profile, bool>> filter = p =>
            (string.IsNullOrEmpty(request.Description) || p.Description.Contains(request.Description)) &&
            (!request.Status.HasValue || p.Status == request.Status.Value);

        var paged = await _profiles.GetPagedAsync(
            request.Page,
            request.PageSize,
            filter,
            p => p.Description,
            ascending: true);

        var response = paged.Items.Select(p => new ProfileResponse
        {
            Id = p.Id,
            Description = p.Description,
            Status = p.Status
        }).ToList();

        return new(response, BuildMeta(request.Page, request.PageSize, paged.TotalCount));
    }

    public async Task<MetaDataResponse<ProfileResponse>> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new MetaDataResponse<ProfileResponse>(null, null);

        var profile = await _profiles.GetByIdAsync(id);
        if (profile == null)
            return new MetaDataResponse<ProfileResponse>(null, null);

        var response = new ProfileResponse
        {
            Id = profile.Id,
            Description = profile.Description,
            Status = profile.Status
        };

        return new(response, null);
    }

    public async Task<MetaDataResponse<bool>> Save(SaveProfileRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            var profile = new Profile
            {
                Id = Guid.NewGuid().ToString(),
                Description = request.Description,
                Status = request.Status,
                CreatedAt = DateTime.Now
            };

            await _profiles.AddAsync(profile);
            return new(true, null);
        }

        var existing = await _profiles.GetByIdAsync(request.Id);
        if (existing == null)
            return new(false, null);

        existing.Description = request.Description;
        existing.Status = request.Status;
        existing.UpdatedAt = DateTime.Now;

        await _profiles.UpdateAsync(existing);
        return new(true, null);
    }

    public async Task<MetaDataResponse<bool>> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new(false, null);

        await _profiles.DeleteAsync(id);
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
