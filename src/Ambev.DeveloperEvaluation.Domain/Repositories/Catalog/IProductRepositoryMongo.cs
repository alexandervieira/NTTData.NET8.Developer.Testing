using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Catalog
{
    public interface IProductRepositoryMongo
    {
        Task<PaginatedList<MongoProduct>> GetAllAsync(int pageNumber, int pageSize, string? query, string order);
        Task AddProductToMongo(Product product);
        Task AddCategoryToMongo(Category category);
    }
}
