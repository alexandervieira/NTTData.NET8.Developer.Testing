using Ambev.DeveloperEvoluation.Core.Messages;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class OrderProductRemovedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }

        public OrderProductRemovedEvent(Guid customerId, Guid orderId, Guid productId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
        }
    }

}
