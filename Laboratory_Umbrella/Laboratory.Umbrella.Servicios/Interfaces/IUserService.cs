using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IUserService
{
    void InitService(CurrentParametersHelpers parameters);
    Task<MetaDataResponse<List<UsuariosResponse>>> GetByParameters(UserByParameters request);
    Task<MetaDataResponse<UsuariosResponse>> GetById(string id);
    Task<MetaDataResponse<bool>> Save(SaveUserRequest request);
    Task<MetaDataResponse<bool>> ChangePassword(string id, ChangePasswordRequest request);
    Task<MetaDataResponse<bool>> ValidatePassword(ValidatePasswordRequest request);
}
