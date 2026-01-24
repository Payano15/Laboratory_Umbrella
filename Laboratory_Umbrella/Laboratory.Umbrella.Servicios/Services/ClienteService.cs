using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Services;

public class ClienteService : BaseService, IClienteService
{
    #region Properties
    private readonly IRepository<Cliente> _repository;
    #endregion

    #region Constructor
    public ClienteService(IRepository<Cliente> repository)
    {
        _repository = repository;
    }
    #endregion

    #region Methods
    public async Task<MetaDataResponse<List<ClientResponse>>> GetClient(ClientRequest request)
    {
        Expression<Func<Cliente, bool>>? filter = null;

        if (!string.IsNullOrEmpty(request.Name))
        {
            filter = c => c.fullName.Contains(request.Name);
        }

        var pagedResult = await _repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            filter,
            c => c.fullName,
            ascending: true);

        var clienteResponse = pagedResult.Items.Select(c => new ClientResponse
        {
            Id = c.Id,
            fullName = c.fullName,
            Email = c.Email,
            Phone = c.Phone,
            gender = c.gender,
            Address = c.Address,
            Status = Enum.GetName(typeof(GeneralStatus.ClientStatus.StatusClient), c.Status) ?? string.Empty,
            bornDate = c.bornDate.ToString("yyyy-MM-dd HH:mm:ss"),
            Audit = new AuditResponse
            {
                UserCreated = c.UserCreated,
                CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UserUpdated = c.UserUpdated,
                UpdatedAt = c.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                UserAnulled = c.UserAnulled,
                AnulledAt = c.AnulledAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            }
        }).ToList();

        var meta = new MetaResponse
        {
            CurrentPage = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / request.PageSize)
        };

        return new(clienteResponse, meta);
    }

    public async Task<MetaDataResponse<ClientResponse>> GetClientById(string Id)
    {
        if (string.IsNullOrEmpty(Id))
            return new MetaDataResponse<ClientResponse>(null, null);

        var cliente = await _repository.GetByIdAsync(Id);

        if (cliente == null)
            return new MetaDataResponse<ClientResponse>(null, null);

        var clienteResponse = new ClientResponse
        {
            Id = cliente.Id,
            fullName = cliente.fullName,
            Email = cliente.Email,
            Phone = cliente.Phone,
            gender = cliente.gender,
            Address = cliente.Address,
            Status = Enum.GetName(typeof(GeneralStatus.ClientStatus.StatusClient), cliente.Status) ?? string.Empty,
            bornDate = cliente.bornDate.ToString("yyyy-MM-dd HH:mm:ss"),
            Audit = new AuditResponse
            {
                UserCreated = cliente.UserCreated,
                CreatedAt = cliente.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UserUpdated = cliente.UserUpdated,
                UpdatedAt = cliente.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                UserAnulled = cliente.UserAnulled,
                AnulledAt = cliente.AnulledAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            }
        };

        return new MetaDataResponse<ClientResponse>(clienteResponse, null);
    }

    public async Task<MetaDataResponse<bool>> CreateOrUpdateClient(CreateClientRequest request)
    {
        try
        {
            var isCreating = string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id);

            if (isCreating)
            {
                var newClient = new Cliente
                {
                    Id = Guid.NewGuid().ToString(),
                    fullName = request.fullName,
                    Email = request.Email,
                    Phone = request.Phone,
                    gender = request.gender,
                    Address = request.Address,
                    Status = (int)GeneralStatus.ClientStatus.StatusClient.ACTIVE,
                    bornDate = request.bornDate,
                    CreatedAt = DateTime.Now,
                    UserCreated = "System"
                };

                await _repository.AddAsync(newClient);
            }
            else
            {
                var existingClient = await _repository.GetByIdAsync(request.Id);

                if (existingClient == null)
                    return new MetaDataResponse<bool>(false, null);

                existingClient.fullName = request.fullName;
                existingClient.Email = request.Email;
                existingClient.Phone = request.Phone;
                existingClient.gender = request.gender;
                existingClient.Address = request.Address;
                existingClient.bornDate = request.bornDate;
                existingClient.UpdatedAt = DateTime.Now;
                existingClient.UserUpdated = "System";

                await _repository.UpdateAsync(existingClient);
            }

            return new MetaDataResponse<bool>(true, null);
        }
        catch (Exception ex)
        {
            return new MetaDataResponse<bool>(false, null);
        }
    }
    #endregion
}