using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface ISeccionesService
{
    Task<MetaDataResponse<List<SeccionesResponse>>> GetByParameters(SeccionesByParameters request);
    Task<MetaDataResponse<SeccionesResponse>> GetById(string id);
    Task<MetaDataResponse<bool>> Save(SaveSeccionesRequest request);
    Task<MetaDataResponse<bool>> Delete(string id);
}
