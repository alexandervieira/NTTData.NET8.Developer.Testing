using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Services
{
    public interface IProductService : IDisposable
    {
        Task<PaginatedList<ProductResponse>> GetAllAsync(int pageNumber, int pageSize, string? query, string order);
        Task<PaginatedList<ProductResponse>> GetAll(int pageNumber, int pageSize, string? query);
        Task<ProductResponse> GetById(Guid id);
        Task<IEnumerable<ProductResponse>> GetByCategory(int code);
        Task<IEnumerable<ProductResponse>> GetByCategoryName(string categoryName);
        Task<IEnumerable<CategoryResponse>> GetCategories();

        Task<ProductResponse> AddProduct(CreateProductRequest request);
        Task<ProductResponse> UpdateProduct(UpdateProductRequest request);
        Task<bool> DeleteProduct(Guid id);

        Task<ProductResponse> DebitStock(Guid id, int quantity);
        Task<ProductResponse> ReplenishStock(Guid id, int quantity);

        Task<CategoryResponse> AddCategory(CategoryRequest request);
        Task<CategoryResponse> UpdateCategory(CategoryRequest request);

    }
    
}
