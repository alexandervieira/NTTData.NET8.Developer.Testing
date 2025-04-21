using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Ambev.DeveloperEvaluation.Domain.Common;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.ORM.Context
{
    public class ProductContext : IProductContext
    {
        private readonly IMediatorHandler _mediatorHandler;
        public IMongoCollection<MongoProduct> Products { get; }
        public IMongoCollection<MongoCategory> Categories { get; }

        public ProductContext(IConfiguration configuration, IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;

            // Configurações de serialização (uma vez)
            if (!BsonClassMap.IsClassMapRegistered(typeof(MongoProduct)))
            {
                BsonClassMap.RegisterClassMap<BaseEntity>(map =>
                {
                    map.SetIsRootClass(true);
                    map.AutoMap();
                    map.MapProperty(t => t.Id).SetSerializer(new GuidSerializer(BsonType.String));
                    map.UnmapProperty(t => t.Notifications);
                });

                BsonClassMap.RegisterClassMap<MongoProduct>(map =>
                {
                    map.AutoMap();
                    map.MapCreator(p => new MongoProduct());
                    map.MapProperty(p => p.CategoryId).SetSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
                });

                BsonClassMap.RegisterClassMap<MongoCategory>(map =>
                {
                    map.AutoMap();
                    map.MapCreator(c => new MongoCategory());
                    //map.UnmapProperty(c => c.Products);
                });
            }

            // Conexão com MongoDB
            var client = new MongoClient(configuration.GetValue<string>("MongoDBSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDBSettings:DatabaseName"));

            Products = database.GetCollection<MongoProduct>(configuration.GetValue<string>("MongoDBSettings:ProductCollection"));

            Categories = database.GetCollection<MongoCategory>(configuration.GetValue<string>("MongoDBSettings:CategoryCollection"));          
           

            // Garante que o índice de texto exista
            CreateTextIndexIfNotExists(Products).GetAwaiter().GetResult();

            CreateTextIndexIfNotExists(Categories).GetAwaiter().GetResult();

            // Seed de dados
            ProductContextSeed.SeedData(Products);
        }

        private async Task CreateTextIndexIfNotExists(IMongoCollection<MongoProduct> collection)
        {
            var indexes = await collection.Indexes.ListAsync();
            var indexList = await indexes.ToListAsync();

            var hasTextIndex = indexList.Any(index =>
                index.Contains("weights") && index["weights"].AsBsonDocument.Names.Contains("Title"));

            if (!hasTextIndex)
            {
                var indexKeys = Builders<MongoProduct>.IndexKeys
                    .Text(p => p.Title)
                    .Text(p => p.Description);

                var model = new CreateIndexModel<MongoProduct>(indexKeys);
                await collection.Indexes.CreateOneAsync(model);
            }
        }

        private async Task CreateTextIndexIfNotExists(IMongoCollection<MongoCategory> collection)
        {
            var indexes = await collection.Indexes.ListAsync();
            var indexList = await indexes.ToListAsync();

            var hasTextIndex = indexList.Any(index =>
                index.Contains("weights") && index["weights"].AsBsonDocument.Names.Contains("Title"));

            if (!hasTextIndex)
            {
                var indexKeys = Builders<MongoCategory>.IndexKeys
                    .Text(p => p.Name)
                    .Text(p => p.Code);

                var model = new CreateIndexModel<MongoCategory>(indexKeys);
                await collection.Indexes.CreateOneAsync(model);
            }
        }

    }
    
}
