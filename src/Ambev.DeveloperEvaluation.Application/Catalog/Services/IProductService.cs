using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Services
{
    public interface IProductService : IDisposable
    {
        Task<IEnumerable<ProductResponse>> GetAll();
        Task<ProductResponse> GetById(Guid id);
        Task<IEnumerable<ProductResponse>> GetByCategory(int code);
        Task<IEnumerable<CategoryResponse>> GetCategories();

        Task<bool> AddProduct(ProductRequest request);
        Task<bool> UpdateProduct(ProductRequest request);               

        Task<ProductResponse> DebitStock(Guid id, int quantity);
        Task<ProductResponse> ReplenishStock(Guid id, int quantity);
    }
    
}
