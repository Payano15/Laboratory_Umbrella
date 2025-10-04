using Microsoft.Extensions.DependencyInjection;
using Laboratory.Umbrella.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace Laboratory.Umbrella.Data;


public static class MongoDependencyContainer
{
    public static IServiceCollection AddMongo(this IServiceCollection service, IConfiguration config)
    {
        MongoMappingConfig.RegisterMappings();
        service.AddSingleton<IMongoClient>(_ => new MongoClient(config["DatabaseConfig:CS"]));
        service.AddScoped<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(config["DatabaseConfig:name"]));
        service.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
        return service;
    }
}
