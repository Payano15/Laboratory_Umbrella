using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Services;

public class SeccionesService : BaseService, ISeccionesService
{
    private readonly IRepository<Secciones> _secciones;

    public SeccionesService(IRepository<Secciones> secciones)
    {
        _secciones = secciones;
    }

    public async Task<MetaDataResponse<List<SeccionesResponse>>> GetByParameters(SeccionesByParameters request)
    {
        Expression<Func<Secciones, bool>> filter = s =>
            (string.IsNullOrEmpty(request.Description) || s.Description.Contains(request.Description)) &&
            (!request.Status.HasValue || s.Status == request.Status.Value);

        var paged = await _secciones.GetPagedAsync(
            request.Page,
            request.PageSize,
            filter,
            s => s.Description,
            ascending: true);

        var response = paged.Items.Select(s => new SeccionesResponse
        {
            Id = s.Id,
            Description = s.Description,
            Icon = s.Icon,
            Order = s.Order,
            Status = s.Status
        }).ToList();

        return new(response, BuildMeta(request.Page, request.PageSize, paged.TotalCount));
    }

    public async Task<MetaDataResponse<SeccionesResponse>> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new MetaDataResponse<SeccionesResponse>(null, null);

        var entity = await _secciones.GetByIdAsync(id);
        if (entity == null)
            return new MetaDataResponse<SeccionesResponse>(null, null);

        var response = new SeccionesResponse
        {
            Id = entity.Id,
            Description = entity.Description,
            Icon = entity.Icon,
            Order = entity.Order,
            Status = entity.Status
        };

        return new(response, null);
    }

    public async Task<MetaDataResponse<bool>> Save(SaveSeccionesRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            var entity = new Secciones
            {
                Id = Guid.NewGuid().ToString(),
                Description = request.Description,
                Icon = request.Icon,
                Order = request.Order,
                Status = request.Status,
                CreatedAt = DateTime.Now
            };

            await _secciones.AddAsync(entity);
            return new(true, null);
        }

        var existing = await _secciones.GetByIdAsync(request.Id);
        if (existing == null)
            return new(false, null);

        existing.Description = request.Description;
        existing.Icon = request.Icon;
        existing.Order = request.Order;
        existing.Status = request.Status;
        existing.UpdatedAt = DateTime.Now;

        await _secciones.UpdateAsync(existing);
        return new(true, null);
    }

    public async Task<MetaDataResponse<bool>> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new(false, null);

        await _secciones.DeleteAsync(id);
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
