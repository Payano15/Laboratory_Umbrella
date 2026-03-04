using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IProfileOptionPermissionService
{
    void InitService(CurrentParametersHelpers parameters);
    Task<MetaDataResponse<List<ProfileOptionPermissionResponse>>> GetByParameters(ProfileOptionPermissionByParameters request);
    Task<MetaDataResponse<ProfileOptionPermissionResponse>> GetById(string id);
    Task<MetaDataResponse<bool>> Save(SaveProfileOptionPermissionRequest request);
    Task<MetaDataResponse<bool>> Delete(string id);
}
