using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Services
{
    public interface IProductService : IDisposable
    {
        Task<IEnumerable<ProductResponse>> GetAll();
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
