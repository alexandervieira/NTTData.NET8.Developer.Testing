using Ambev.DeveloperEvaluation.Domain.Events.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.Notifications;

namespace Ambev.DeveloperEvaluation.Domain.Services.Catalog
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public StockService(IProductRepository productRepository, 
                            IMediatorHandler mediatorHandler)
        {
            _productRepository = productRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> DebitStock(Guid productId, int quantity)
        {
            if (!await DebitItemStock(productId, quantity)) return false;
            return await _productRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> ReplenishStock(Guid productId, int quantity)
        {
            var success = await ReplenishItemStock(productId, quantity);
            if (!success) return false;
            return await _productRepository.UnitOfWork.CommitAsync();
        }        

        public async Task<bool> DebitListProductsOrder(ListProductsOrder collection)
        {
            if (collection == null || collection.Items == null || !collection.Items.Any())
            {
                return false;
            }
            foreach (var item in collection.Items)
            {                             
                if(!await DebitItemStock(item.Id, item.Quantity)) return false;
            }
            return await _productRepository.UnitOfWork.CommitAsync();
        }

        public Task<bool> ReplenishListProductsOrder(ListProductsOrder collection)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> DebitItemStock(Guid productId, int quantity)
        {
            var product = await _productRepository.ObterPorId(productId);
            if (product == null) return false;

            if (!product.HasStock(quantity))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("Stock", $"Insufficient stock - Product: {product.Title}"));
                return false;
            }

            product.DebitStock(quantity);
            if (product.QuantityStock < 10)
            {
                await _mediatorHandler.PublishDomainEvent(new ProductLowStockEvent(product.Id, product.QuantityStock));
            }

            _productRepository.UpdateProduct(product);

            return true;
        }

        private async Task<bool> ReplenishItemStock(Guid productId, int quantity)
        {
            var product = await _productRepository.ObterPorId(productId);
            if (product == null) return false;
            product.ReplenishStock(quantity);
            _productRepository.UpdateProduct(product);
            return true;
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
        
    }
}
