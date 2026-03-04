using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IClienteService
{
    void InitService(CurrentParametersHelpers parameters);
    Task<MetaDataResponse<List<ClientResponse>>> GetClient(ClientRequest request);
    Task<MetaDataResponse<ClientResponse>> GetClientById(string Id);
    Task<MetaDataResponse<bool>> CreateOrUpdateClient(CreateClientRequest request);
}
