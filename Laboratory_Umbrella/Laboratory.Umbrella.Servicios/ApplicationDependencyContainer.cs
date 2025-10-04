using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Servicios;

public static class ApplicationDependencyContainer
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        // Aquí puedes registrar los servicios de la capa de aplicación
        // Por ejemplo:
        // services.AddTransient<IMiServicio, MiServicio>();
        return services;
    }
}
