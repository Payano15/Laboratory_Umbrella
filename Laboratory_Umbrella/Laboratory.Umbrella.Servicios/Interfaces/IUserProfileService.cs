using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IUserProfileService
{
    Task<MetaDataResponse<List<UserProfileResponse>>> GetByParameters(UserProfileByParameters request);
    Task<MetaDataResponse<UserProfileResponse>> GetById(string id);
    Task<MetaDataResponse<bool>> Save(SaveUserProfileRequest request);
    Task<MetaDataResponse<bool>> Delete(string id);
}
