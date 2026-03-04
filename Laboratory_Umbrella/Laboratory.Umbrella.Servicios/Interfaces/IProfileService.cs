using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IProfileService
{
    void InitService(CurrentParametersHelpers parameters);
    Task<MetaDataResponse<List<ProfileResponse>>> GetByParameters(ProfileByParameters request);
    Task<MetaDataResponse<ProfileResponse>> GetById(string id);
    Task<MetaDataResponse<bool>> Save(SaveProfileRequest request);
    Task<MetaDataResponse<bool>> Delete(string id);
}
