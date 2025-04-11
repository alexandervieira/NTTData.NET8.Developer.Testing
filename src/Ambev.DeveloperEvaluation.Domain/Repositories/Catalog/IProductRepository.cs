using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvoluation.Core.Data;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Catalog
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> ObterPorId(Guid id);
        Task<IEnumerable<Product>> GetByCategory(int code);
        Task<IEnumerable<Category>> GetCategories();

        void AddProduct(Product product);
        void UpdateProduct(Product product);

        void AddCategory(Category category);
        void UpdateCategory(Category category);
    }
}
