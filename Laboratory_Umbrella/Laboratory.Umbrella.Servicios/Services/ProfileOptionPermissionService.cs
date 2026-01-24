using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Services;

public class ProfileOptionPermissionService : BaseService, IProfileOptionPermissionService
{
    private readonly IRepository<ProfileOptionPermission> _permissions;

    public ProfileOptionPermissionService(IRepository<ProfileOptionPermission> permissions)
    {
        _permissions = permissions;
    }

    public async Task<MetaDataResponse<List<ProfileOptionPermissionResponse>>> GetByParameters(ProfileOptionPermissionByParameters request)
    {
        Expression<Func<ProfileOptionPermission, bool>> filter = p =>
            (string.IsNullOrEmpty(request.ProfileId) || p.ProfileId == request.ProfileId) &&
            (string.IsNullOrEmpty(request.OptionId) || p.OptionId == request.OptionId) &&
            (!request.PermissionId.HasValue || p.PermissionId == request.PermissionId.Value) &&
            (!request.Granted.HasValue || p.Granted == request.Granted.Value);

        var paged = await _permissions.GetPagedAsync(
            request.Page,
            request.PageSize,
            filter,
            p => p.ProfileId,
            ascending: true);

        var response = paged.Items.Select(p => new ProfileOptionPermissionResponse
        {
            Id = p.Id,
            ProfileId = p.ProfileId,
            OptionId = p.OptionId,
            PermissionId = p.PermissionId,
            Granted = p.Granted
        }).ToList();

        return new(response, BuildMeta(request.Page, request.PageSize, paged.TotalCount));
    }

    public async Task<MetaDataResponse<ProfileOptionPermissionResponse>> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new MetaDataResponse<ProfileOptionPermissionResponse>(null, null);

        var entity = await _permissions.GetByIdAsync(id);
        if (entity == null)
            return new MetaDataResponse<ProfileOptionPermissionResponse>(null, null);

        var response = new ProfileOptionPermissionResponse
        {
            Id = entity.Id,
            ProfileId = entity.ProfileId,
            OptionId = entity.OptionId,
            PermissionId = entity.PermissionId,
            Granted = entity.Granted
        };

        return new(response, null);
    }

    public async Task<MetaDataResponse<bool>> Save(SaveProfileOptionPermissionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            var entity = new ProfileOptionPermission
            {
                Id = Guid.NewGuid().ToString(),
                ProfileId = request.ProfileId,
                OptionId = request.OptionId,
                PermissionId = request.PermissionId,
                Granted = request.Granted,
                CreatedAt = DateTime.Now
            };

            await _permissions.AddAsync(entity);
            return new(true, null);
        }

        var existing = await _permissions.GetByIdAsync(request.Id);
        if (existing == null)
            return new(false, null);

        existing.ProfileId = request.ProfileId;
        existing.OptionId = request.OptionId;
        existing.PermissionId = request.PermissionId;
        existing.Granted = request.Granted;
        existing.UpdatedAt = DateTime.Now;

        await _permissions.UpdateAsync(existing);
        return new(true, null);
    }

    public async Task<MetaDataResponse<bool>> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new(false, null);

        await _permissions.DeleteAsync(id);
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
