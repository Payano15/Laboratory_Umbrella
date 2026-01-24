using Microsoft.Extensions.DependencyInjection;
using Laboratory.Umbrella.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Laboratory.Umbrella.Data;

public static class SQLDependencyContainer
{
    public static IServiceCollection AddSqlServer(this IServiceCollection service, IConfiguration config)
    {
        service.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = config["DatabaseConfig:CS"];

            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(30);
            });

            var enableSensitiveLoggingRaw = config["DatabaseConfig:EnableSensitiveDataLogging"];
            if (bool.TryParse(enableSensitiveLoggingRaw, out var enableSensitiveLogging) && enableSensitiveLogging)
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        service.AddScoped(typeof(IRepository<>), typeof(SQLDBRepository<>));

        service.AddScoped<DbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return service;
    }

    public static IServiceCollection AddSqlServerWithMigration(this IServiceCollection service, IConfiguration config)
    {
        service.AddSqlServer(config);

        var serviceProvider = service.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        return service;
    }
}