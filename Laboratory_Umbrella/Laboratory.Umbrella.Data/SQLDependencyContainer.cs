using Microsoft.Extensions.DependencyInjection;
using Laboratory.Umbrella.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Laboratory.Umbrella.Data;

public static class SQLDependencyContainer
{
    public static IServiceCollection AddSqlServer(this IServiceCollection service, IConfiguration config)
    {
        // Registrar el DbContext con SQL Server
        service.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = config["DatabaseConfig:CS"];

            options.UseSqlServer(connectionString, sqlOptions =>
            {
                // Configuraciones adicionales opcionales
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(30); // Timeout en segundos
            });

            // Solo para desarrollo - muestra queries en consola
            if (config.GetValue<bool>("DatabaseConfig:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // Registrar el repositorio genérico
        service.AddScoped(typeof(IRepository<>), typeof(SQLDBRepository<>));

        // Opcional: Registrar DbContext como inyección directa también
        service.AddScoped<DbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return service;
    }

    // Método adicional para migración automática (usar solo en desarrollo)
    public static IServiceCollection AddSqlServerWithMigration(this IServiceCollection service, IConfiguration config)
    {
        service.AddSqlServer(config);

        // Aplicar migraciones automáticamente al iniciar
        var serviceProvider = service.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        return service;
    }
}