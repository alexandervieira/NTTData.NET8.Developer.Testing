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
        public IMongoCollection<Product> Products { get; }

        public ProductContext(IConfiguration configuration, IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;

            // Configurações de serialização (uma vez)
            if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
            {
                BsonClassMap.RegisterClassMap<BaseEntity>(map =>
                {
                    map.SetIsRootClass(true);
                    map.AutoMap();
                    map.MapProperty(t => t.Id).SetSerializer(new GuidSerializer(BsonType.String));
                    map.UnmapProperty(t => t.Notifications);
                });

                BsonClassMap.RegisterClassMap<Product>(map =>
                {
                    map.AutoMap();
                    map.MapCreator(p => new Product());
                    map.MapProperty(p => p.CategoryId).SetSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
                });

                BsonClassMap.RegisterClassMap<Category>(map =>
                {
                    map.AutoMap();
                    map.MapCreator(c => new Category());
                    map.UnmapProperty(c => c.Products);
                });
            }

            // Conexão com MongoDB
            var client = new MongoClient(configuration.GetValue<string>("MongoDBSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDBSettings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("MongoDBSettings:CollectionName"));

            // Garante que o índice de texto exista
            CreateTextIndexIfNotExists(Products).GetAwaiter().GetResult();

            // Seed de dados
            ProductContextSeed.SeedData(Products);
        }

        private async Task CreateTextIndexIfNotExists(IMongoCollection<Product> collection)
        {
            var indexes = await collection.Indexes.ListAsync();
            var indexList = await indexes.ToListAsync();

            var hasTextIndex = indexList.Any(index =>
                index.Contains("weights") && index["weights"].AsBsonDocument.Names.Contains("Title"));

            if (!hasTextIndex)
            {
                var indexKeys = Builders<Product>.IndexKeys
                    .Text(p => p.Title)
                    .Text(p => p.Description);

                var model = new CreateIndexModel<Product>(indexKeys);
                await collection.Indexes.CreateOneAsync(model);
            }
        }
    }


    //public class MongoDBContext
    //{
    //    public class ProductContext : IProductContext
    //    {
    //        private readonly IMediatorHandler _mediatorHandler;
    //        public IMongoCollection<Product> Products { get; }

    //        public ProductContext(IConfiguration configuration, IMediatorHandler mediatorHandler)
    //        {
    //            _mediatorHandler = mediatorHandler;

    //            if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
    //            {
    //                BsonClassMap.RegisterClassMap<BaseEntity>(map =>
    //                {
    //                    map.SetIsRootClass(true);
    //                    map.AutoMap();
    //                    map.MapProperty(t => t.Id).SetSerializer(new GuidSerializer(BsonType.String));
    //                    map.UnmapProperty(t => t.Notifications);                       
    //                });

    //                BsonClassMap.RegisterClassMap<Product>(map =>
    //                {
    //                    map.AutoMap();
    //                    map.MapCreator(p => new Product());
    //                    map.MapProperty(p => p.CategoryId).SetSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

    //                });

    //                BsonClassMap.RegisterClassMap<Category>(map =>
    //                {
    //                    map.AutoMap();
    //                    map.MapCreator(c => new Category());
    //                    //map.MapProperty(c => c.Id).SetSerializer(new GuidSerializer(BsonType.String));
    //                    map.UnmapProperty(c => c.Products);
    //                });

    //            }

    //            var client = new MongoClient(configuration.GetValue<string>("MongoDBSettings:ConnectionString"));
    //            var database = client.GetDatabase(configuration.GetValue<string>("MongoDBSettings:DatabaseName"));

    //            Products = database.GetCollection<Product>(configuration.GetValue<string>("MongoDBSettings:CollectionName"));
    //            ProductContextSeed.SeedData(Products);
    //        }

    //    }      

    //}
}
