using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events.Handlers
{
    public class OrderEventHandler :
        INotificationHandler<OrderDraftStartedEvent>,
        INotificationHandler<OrderItemAddedEvent>,
        INotificationHandler<OrderStockRejectedEvent>,
        INotificationHandler<OrderProductAddedEvent>,
        INotificationHandler<OrderProductRemovedEvent>,
        INotificationHandler<OrderProductUpdatedEvent>,     
        INotificationHandler<OrderPaymentCompletedEvent>,
        INotificationHandler<OrderPaymentRejectedEvent>,
        INotificationHandler<VoucherAppliedToOrderEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ILogger<OrderEventHandler> _logger;

        public OrderEventHandler(IMediatorHandler mediatorHandler, ILogger<OrderEventHandler> logger)
        {
            _mediatorHandler = mediatorHandler;
            _logger = logger;
        }

        public Task Handle(OrderDraftStartedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("OrderDraftStartedEvent handled for OrderId: {OrderId}, CustomerId: {CustomerId}", 
                                    notification.OrderId, notification.CustomerId);
            return Task.CompletedTask;
        }

        public Task Handle(OrderItemAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("OrderItemAddedEvent handled for OrderId: {OrderId}, CustomerId: {CustomerId}, ProductId: {ProductId}", 
                                    notification.OrderId, notification.CustomerId, notification.ProductId);
            return Task.CompletedTask;
        }

        public async Task Handle(OrderStockRejectedEvent message, CancellationToken cancellationToken)
        {
            _logger.LogWarning("OrderStockRejectedEvent handled for OrderId: {OrderId}, CustomerId: {CustomerId}", 
                                message.OrderId, message.CustomerId);
            await _mediatorHandler.SendCommand(new CancelOrderProcessingCommand(message.OrderId, message.CustomerId));
        }

        public async Task Handle(OrderPaymentCompletedEvent message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("OrderPaymentCompletedEvent handled for OrderId: {OrderId}, CustomerId: {CustomerId}", 
                                    message.OrderId, message.CustomerId);
            await _mediatorHandler.SendCommand(new FinalizeOrderCommand(message.OrderId, message.CustomerId));
        }

        public async Task Handle(OrderPaymentRejectedEvent message, CancellationToken cancellationToken)
        {
            _logger.LogError("OrderPaymentRejectedEvent handled for OrderId: {OrderId}, CustomerId: {CustomerId}", 
                             message.OrderId, message.CustomerId);
            await _mediatorHandler.SendCommand(new CancelOrderProcessingAndRestockCommand(message.OrderId, message.CustomerId));
        }

        public Task Handle(VoucherAppliedToOrderEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"VoucherAppliedToOrderEvent handled for OrderId: {notification.OrderId}, " +
                                   $"CustomerId: {notification.CustomerId}, VoucherCode: {notification.VoucherId}");
            return Task.CompletedTask;
        }

        public Task Handle(OrderProductAddedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(OrderProductRemovedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(OrderProductUpdatedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
       
    }

}
