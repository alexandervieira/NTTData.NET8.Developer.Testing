namespace Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents
{
    public class OrderStockRejectedEvent : IntegrationEvent
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }

        public OrderStockRejectedEvent(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }
    }

}
