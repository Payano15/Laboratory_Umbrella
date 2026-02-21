using Laboratory.Umbrella.Services.Interfaces;
using Laboratory.Umbrella.Services.Services;
using Laboratory.Umbrella.Services.Services.Autentications;
using Microsoft.Extensions.DependencyInjection;

namespace Laboratory.Umbrella.Servicios;

public static class ApplicationDependencyContainer
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IOpcionesService, OpcionesService>();
        services.AddScoped<ISeccionesService, SeccionesService>();
        services.AddScoped<IProfileOptionPermissionService, ProfileOptionPermissionService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
