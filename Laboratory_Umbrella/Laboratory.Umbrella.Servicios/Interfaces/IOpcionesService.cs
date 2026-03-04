using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IOpcionesService
{
    void InitService(CurrentParametersHelpers parameters);
    Task<MetaDataResponse<List<OpcionesResponse>>> GetByParameters(OpcionesByParameters request);
    Task<MetaDataResponse<OpcionesResponse>> GetById(string id);
    Task<MetaDataResponse<bool>> Save(SaveOpcionesRequest request);
    Task<MetaDataResponse<bool>> Delete(string id);
}
