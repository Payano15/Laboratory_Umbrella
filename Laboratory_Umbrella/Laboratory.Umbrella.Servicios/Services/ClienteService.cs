using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;

namespace Laboratory.Umbrella.Services.Services;

public class ClienteService : BaseService, IClienteService
{
    #region Constructor
    private readonly IRepository<Cliente> _repository;
    #endregion

    #region properties
    public ClienteService(IRepository<Cliente> repository)
    {
        _repository = repository;
    }
    #endregion

    #region Methods
    public async Task<MetaDataResponse<List<ClientResponse>,MetaResponse>> GetClient(ClientRequest request)
    {
        var filter = new List<FilterDefinition<Cliente>>();
        if (!string.IsNullOrEmpty(request.Name))
            filter.Add(Builders<Cliente>.Filter.Eq(c => c.fullName, request.Name));

        var CombinedFilter = filter.Count > 0
            ? Builders<Cliente>.Filter.And(filter)
            : Builders<Cliente>.Filter.Empty;

        var TotalRecords = await _repository.Collection.CountDocumentsAsync(CombinedFilter);
        var skip = (request.PageNumber - 1) * request.PageSize;

        var clientes = await _repository.Collection.Aggregate()
            .Match(CombinedFilter)
            .Skip(skip)
            .Limit(request.PageSize)
            .ToListAsync();

        var ClienteResponse = clientes.Select(c => new ClientResponse
        {
            Id = c.Id,
            fullName = c.fullName,
            Email = c.Email,
            Phone = c.Phone,
            gender = c.gender,
            Address = c.Address,
            Status = Enum.GetName(typeof(GeneralStatus.ClientStatus.StatusClient), c.Status) ?? string.Empty,
            bornDate = c.bornDate.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            Audit = new AuditResponse
            {
                UserCreated = c.UserCreated,
                CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                UserUpdated = c.UserUpdated,
                UpdatedAt = c.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                UserAnulled = c.UserAnulled,
                AnulledAt = c.AnulledAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            }
        }).ToList();

        var meta = new MetaResponse
        {
            CurrentPage = (int)TotalRecords,
            PageSize = request.PageSize,
            TotalCount = request.PageNumber,
            TotalPages = (int)Math.Ceiling((double)TotalRecords / request.PageSize)
        };

        return new MetaDataResponse<List<ClientResponse>, MetaResponse>(ClienteResponse,meta);

    }
    public async Task<MetaDataResponse<ClientResponse, MetaResponse>> GetClientById(string Id)
    {
        if(!string.IsNullOrEmpty(Id))
            return new MetaDataResponse<ClientResponse, MetaResponse>(null, null);

        var Cliente = await _repository.Collection.AsQueryable().FirstOrDefaultAsync(c => c.Id == Id);
        if (Cliente == null)
            return new MetaDataResponse<ClientResponse, MetaResponse>(null, null);

        var ClienteResponse = new ClientResponse()
        {
            Id = Cliente.Id,
            fullName = Cliente.fullName,
            Email = Cliente.Email,
            Phone = Cliente.Phone,
            gender = Cliente.gender,
            Address = Cliente.Address,
            Status = Enum.GetName(typeof(GeneralStatus.ClientStatus.StatusClient), Cliente.Status) ?? string.Empty,
            bornDate = Cliente.bornDate.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            Audit = new AuditResponse
            {
                UserCreated = Cliente.UserCreated,
                CreatedAt = Cliente.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                UserUpdated = Cliente.UserUpdated,
                UpdatedAt = Cliente.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                UserAnulled = Cliente.UserAnulled,
                AnulledAt = Cliente.AnulledAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            }
        };

        return new MetaDataResponse<ClientResponse, MetaResponse>(ClienteResponse, null);
    }
    public async Task<MetaDataResponse<bool, MetaResponse>> CreateOrUpdateClient(CreateClientRequest request)
    {
        try
        {
            var Iscreating = string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id);

            if (Iscreating)
            {
                var newClient = new Cliente
                {
                    fullName = request.fullName,
                    Email = request.Email,
                    Phone = request.Phone,
                    gender = request.gender,
                    Address = request.Address,
                    Status = (int)GeneralStatus.ClientStatus.StatusClient.ACTIVE,
                    bornDate = request.bornDate
                };

                await _repository.AddAsync(newClient);
            }
            else
            {
                var existingClient = await _repository.Collection.AsQueryable().FirstOrDefaultAsync(c => c.Id == request.Id);
                if (existingClient == null)
                    return new MetaDataResponse<bool, MetaResponse>(false, null);

                existingClient.fullName = request.fullName;
                existingClient.Email = request.Email;
                existingClient.Phone = request.Phone;
                existingClient.gender = request.gender;
                existingClient.Address = request.Address;
                existingClient.bornDate = request.bornDate;

                await _repository.UpdateAsync(existingClient);

            }
            ;

            return new MetaDataResponse<bool, MetaResponse>(true, null);
        }
        catch (Exception ex)
        {
            return new MetaDataResponse<bool, MetaResponse>(false, null);
        }
    }
    #endregion
}
