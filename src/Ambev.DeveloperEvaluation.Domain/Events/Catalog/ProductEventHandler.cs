using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents;
using Ambev.DeveloperEvoluation.Domain.Events.Catalog;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Catalog
{
    public class ProductEventHandler :
    INotificationHandler<ProductLowStockEvent>,
    INotificationHandler<ProductCreatedEvent>,
    INotificationHandler<OrderStartedEvent>,
    INotificationHandler<OrderProcessingCancelledEvent>
    {
        private readonly IProductRepositoryMongo _productRepositoryMongo;
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMediatorHandler _mediatorHandler;

        public ProductEventHandler(IProductRepositoryMongo productRepositoryMongo, IProductRepository productRepository, 
                                   IStockService stockService, IMediatorHandler mediatorHandler)
        {
            _productRepositoryMongo = productRepositoryMongo;
            _productRepository = productRepository;
            _stockService = stockService;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(ProductLowStockEvent message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(message.AggregateId);

            // Send an email to acquire more products.
        }

        public async Task Handle(OrderStartedEvent message, CancellationToken cancellationToken)
        {
            var result = await _stockService.DebitListProductsOrder(message.OrderProducts);

            if (result)
            {
                await _mediatorHandler.PublishEvent(new OrderStockConfirmedEvent(
                    message.OrderId,
                    message.CustomerId,
                    message.Total,
                    message.OrderProducts,
                    message.CardHolderName,
                    message.CardNumber,
                    message.CardExpiration,
                    message.CardCvv
                ));
            }
            else
            {
                await _mediatorHandler.PublishEvent(new OrderStockRejectedEvent(message.OrderId, message.CustomerId));
            }
        }

        public async Task Handle(OrderProcessingCancelledEvent message, CancellationToken cancellationToken)
        {
            await _stockService.ReplenishListProductsOrder(message.OrderProducts);
        }

        public async Task Handle(ProductCreatedEvent message, CancellationToken cancellationToken)
        {          

            try
            {                              
                if (message == null)
                    throw new DomainException("Product not found.");

                // Adicionar a categoria no MongoDB
                await _productRepositoryMongo.AddCategoryToMongo(message.Product.Category);

                // Adicionar o produto no MongoDB
                await _productRepositoryMongo.AddProductToMongo(message.Product);               

            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("Erro ao tentar sincronizar base de dados.");
            }           
            
        }
    }

}
