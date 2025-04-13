using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events.Handlers
{
    public class OrderEventHandler :
    INotificationHandler<OrderDraftStartedEvent>,
    INotificationHandler<OrderItemAddedEvent>,
    INotificationHandler<OrderStockRejectedEvent>,
    INotificationHandler<OrderPaymentCompletedEvent>,
    INotificationHandler<OrderPaymentRejectedEvent>
    {

        private readonly IMediatorHandler _mediatorHandler;

        public OrderEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public Task Handle(OrderDraftStartedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(OrderItemAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(OrderStockRejectedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new CancelOrderProcessingCommand(message.OrderId, message.CustomerId));
        }

        public async Task Handle(OrderPaymentCompletedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new FinalizeOrderCommand(message.OrderId, message.CustomerId));
        }

        public async Task Handle(OrderPaymentRejectedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new CancelOrderProcessingAndRestockCommand(message.OrderId, message.CustomerId));
        }
    }

}
