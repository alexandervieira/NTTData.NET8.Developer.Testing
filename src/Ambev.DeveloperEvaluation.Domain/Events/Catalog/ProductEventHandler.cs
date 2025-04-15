using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Catalog
{
    public class ProductEventHandler :
    INotificationHandler<ProductLowStockEvent>,
    INotificationHandler<OrderStartedEvent>,
    INotificationHandler<OrderProcessingCancelledEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMediatorHandler _mediatorHandler;

        public ProductEventHandler(IProductRepository productRepository,
                                   IStockService stockService,
                                   IMediatorHandler mediatorHandler)
        {
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
    }

}
