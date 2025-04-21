using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Context
{
    public interface IProductContext
    {
        IMongoCollection<MongoProduct> Products { get; }
        IMongoCollection<MongoCategory> Categories { get; }
    }
}
