using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IClienteService
{
    Task<MetaDataResponse<List<ClientResponse>, MetaResponse>> GetClient(ClientRequest request);
    Task<MetaDataResponse<ClientResponse, MetaResponse>> GetClientById(string Id);
    Task<MetaDataResponse<bool, MetaResponse>> CreateOrUpdateClient(CreateClientRequest request);
}
