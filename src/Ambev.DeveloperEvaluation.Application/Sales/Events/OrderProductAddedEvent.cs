using Ambev.DeveloperEvoluation.Core.Messages;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class OrderProductAddedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public OrderProductAddedEvent(Guid customerId, Guid orderId, Guid productId, int quantity)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

}
