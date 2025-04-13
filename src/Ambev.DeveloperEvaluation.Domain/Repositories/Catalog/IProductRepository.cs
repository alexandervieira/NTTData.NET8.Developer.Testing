using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvoluation.Core.Data;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Catalog
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetByCategory(int code);
        Task<IEnumerable<Product>> GetByCategoryName(string categoryName);
        Task<IEnumerable<Category>> GetCategories();

        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        void DeleteProduct(Guid id);

        Task<Category> AddCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        
    }
}
