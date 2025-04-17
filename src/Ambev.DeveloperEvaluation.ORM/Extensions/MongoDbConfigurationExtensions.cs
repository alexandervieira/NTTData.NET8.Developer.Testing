using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Extensions
{
    public static class MongoDbConfigurationExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Define a representação padrão para GUIDs como "Standard"
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
#pragma warning restore CS0618 // O tipo ou membro é obsoleto

            var connectionString = configuration.GetValue<string>("MongoDBSettings:ConnectionString");
            var databaseName = configuration.GetValue<string>("MongoDBSettings:DatabaseName");

            var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
            mongoClientSettings.GuidRepresentation = GuidRepresentation.Standard;
#pragma warning restore CS0618 // O tipo ou membro é obsoleto

            var mongoClient = new MongoClient(mongoClientSettings);
            var database = mongoClient.GetDatabase(databaseName);

            // Registrar o IMongoDatabase como singleton
            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton(database);

            return services;
        }
    }
}
