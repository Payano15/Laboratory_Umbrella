using Laboratory.Umbrella.Data;
using Laboratory.Umbrella.Dominio.Common;

namespace Laboratory.Umbrella.Api;

public static class InfrastructureConfig
{
    public static IServiceCollection AddConfiguredDatabase(
    this IServiceCollection services, IConfiguration config)
    {
        var db = config["DatabaseConfig:Type"]?.ToLowerInvariant();

        return db switch
        {
            nameof(Types.Database.mongo) => services.AddMongo(config),
            _ => throw new ArgumentOutOfRangeException(nameof(db),
                $"Invalid database: {db}.")
        };

    }
}
