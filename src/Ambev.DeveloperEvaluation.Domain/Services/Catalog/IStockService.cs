using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;

namespace Ambev.DeveloperEvaluation.Domain.Services.Catalog
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStock(Guid productId, int quantity);
        Task<bool> DebitListProductsOrder(ListProductsOrder collection);
        Task<bool> ReplenishStock(Guid productId, int quantity);
        Task<bool> ReplenishListProductsOrder(ListProductsOrder collection);
    }
}
