using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IAuthenticationService
{
    void InitService(CurrentParametersHelpers parameters);
    Task<LoginResponse> Login(LoginRequest request);
}