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
            bornDate = DateToString(c.bornDate),
            TypeClient = Enum.GetName(typeof(GeneralStatus.TypeClient.TypeClients), c.TypeClient) ?? string.Empty,
            CreditLimit = c.CreditLimit,
            discount = c.discount,
            Audit = new AuditResponse
            {
                UserCreated = c.UserCreated,
                CreatedAt = DateToString(c.CreatedAt),
                UserUpdated = c.UserUpdated,
                UpdatedAt = DateToString(c.UpdatedAt),
                UserAnulled = c.UserAnulled,
                AnulledAt = DateToString(c.AnulledAt),
            }
        }).ToList();

        return new(clienteResponse, BuildMeta(request.PageNumber, request.PageSize, pagedResult.TotalCount));
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
            bornDate = DateToString(cliente.bornDate),
            TypeClient = Enum.GetName(typeof(GeneralStatus.TypeClient.TypeClients), cliente.TypeClient) ?? string.Empty,
            CreditLimit = cliente.CreditLimit,
            discount = cliente.discount,
            Audit = new AuditResponse
            {
                UserCreated = cliente.UserCreated,
                CreatedAt = DateToString(cliente.CreatedAt),
                UserUpdated = cliente.UserUpdated,
                UpdatedAt = DateToString(cliente.UpdatedAt),
                UserAnulled = cliente.UserAnulled,
                AnulledAt = DateToString(cliente.AnulledAt),
            }
        };

        return new(clienteResponse);
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
                    TypeClient = (int)GeneralStatus.TypeClient.TypeClients.Client,
                    CreditLimit = request.CreditLimit,
                    discount = request.discount,
                    CreatedAt = DateTime.Now,
                    UserCreated = UserLogged
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
                existingClient.Status = request.Status;
                existingClient.CreditLimit = request.CreditLimit;
                existingClient.discount = request.discount;
                existingClient.bornDate = request.bornDate;
                existingClient.UpdatedAt = DateTime.Now;
                existingClient.UserUpdated = UserLogged;

                await _repository.UpdateAsync(existingClient);
            }

            return new(true);
        }
        catch (Exception ex)
        {
            return new(false);
        }
    }
    #endregion

    #region Auxiliary Methods
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
    #endregion
}