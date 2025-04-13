using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;

namespace Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents
{
    public class OrderProcessingCancelledEvent : IntegrationEvent
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public OrderProductsList OrderProducts { get; private set; }

        public OrderProcessingCancelledEvent(Guid orderId, Guid customerId, OrderProductsList orderProducts)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            OrderProducts = orderProducts;
        }
    }

}
