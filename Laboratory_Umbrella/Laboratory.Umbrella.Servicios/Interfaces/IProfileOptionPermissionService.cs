using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IProfileOptionPermissionService
{
    Task<MetaDataResponse<List<ProfileOptionPermissionResponse>>> GetByParameters(ProfileOptionPermissionByParameters request);
    Task<MetaDataResponse<ProfileOptionPermissionResponse>> GetById(string id);
    Task<MetaDataResponse<bool>> Save(SaveProfileOptionPermissionRequest request);
    Task<MetaDataResponse<bool>> Delete(string id);
}
