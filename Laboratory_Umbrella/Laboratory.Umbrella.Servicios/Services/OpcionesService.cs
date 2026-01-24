using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Services;

public class OpcionesService : BaseService, IOpcionesService
{
    private readonly IRepository<Opciones> _opciones;

    public OpcionesService(IRepository<Opciones> opciones)
    {
        _opciones = opciones;
    }

    public async Task<MetaDataResponse<List<OpcionesResponse>>> GetByParameters(OpcionesByParameters request)
    {
        Expression<Func<Opciones, bool>> filter = o =>
            (string.IsNullOrEmpty(request.Description) || o.Description.Contains(request.Description)) &&
            (string.IsNullOrEmpty(request.Type) || o.Type == request.Type) &&
            (string.IsNullOrEmpty(request.SectionId) || o.SectionId == request.SectionId) &&
            (!request.Status.HasValue || o.Status == request.Status.Value);

        var paged = await _opciones.GetPagedAsync(
            request.Page,
            request.PageSize,
            filter,
            o => o.Description,
            ascending: true);

        var response = paged.Items.Select(o => new OpcionesResponse
        {
            Id = o.Id,
            Description = o.Description,
            Type = o.Type,
            Url = o.Url,
            SectionId = o.SectionId,
            Order = o.Order,
            Visible = o.Visible,
            Status = o.Status
        }).ToList();

        return new(response, BuildMeta(request.Page, request.PageSize, paged.TotalCount));
    }

    public async Task<MetaDataResponse<OpcionesResponse>> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new MetaDataResponse<OpcionesResponse>(null, null);

        var entity = await _opciones.GetByIdAsync(id);
        if (entity == null)
            return new MetaDataResponse<OpcionesResponse>(null, null);

        var response = new OpcionesResponse
        {
            Id = entity.Id,
            Description = entity.Description,
            Type = entity.Type,
            Url = entity.Url,
            SectionId = entity.SectionId,
            Order = entity.Order,
            Visible = entity.Visible,
            Status = entity.Status
        };

        return new(response, null);
    }

    public async Task<MetaDataResponse<bool>> Save(SaveOpcionesRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            var entity = new Opciones
            {
                Id = Guid.NewGuid().ToString(),
                Description = request.Description,
                Type = request.Type,
                Url = request.Url,
                SectionId = request.SectionId,
                Order = request.Order,
                Visible = request.Visible,
                Status = request.Status,
                CreatedAt = DateTime.Now
            };

            await _opciones.AddAsync(entity);
            return new(true, null);
        }

        var existing = await _opciones.GetByIdAsync(request.Id);
        if (existing == null)
            return new(false, null);

        existing.Description = request.Description;
        existing.Type = request.Type;
        existing.Url = request.Url;
        existing.SectionId = request.SectionId;
        existing.Order = request.Order;
        existing.Visible = request.Visible;
        existing.Status = request.Status;
        existing.UpdatedAt = DateTime.Now;

        await _opciones.UpdateAsync(existing);
        return new(true, null);
    }

    public async Task<MetaDataResponse<bool>> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new(false, null);

        await _opciones.DeleteAsync(id);
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
