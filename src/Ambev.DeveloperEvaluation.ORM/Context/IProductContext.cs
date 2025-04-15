using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Context
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
