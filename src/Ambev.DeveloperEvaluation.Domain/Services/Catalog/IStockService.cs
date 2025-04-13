using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;

namespace Ambev.DeveloperEvaluation.Domain.Services.Catalog
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStock(Guid productId, int quantity);
        Task<bool> DebitListProductsOrder(OrderProductsList collection);
        Task<bool> ReplenishStock(Guid productId, int quantity);
        Task<bool> ReplenishListProductsOrder(OrderProductsList collection);
    }
}
