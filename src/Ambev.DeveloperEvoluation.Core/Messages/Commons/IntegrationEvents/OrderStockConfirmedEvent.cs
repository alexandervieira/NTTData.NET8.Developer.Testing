using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;

namespace Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents
{
    public class OrderStockConfirmedEvent : IntegrationEvent
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public decimal Total { get; private set; }
        public OrderProductsList OrderProducts { get; private set; }
        public string CardHolderName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpiration { get; private set; }
        public string CardCvv { get; private set; }

        public OrderStockConfirmedEvent(Guid orderId, Guid customerId, decimal total, OrderProductsList orderProducts, string cardHolderName, string cardNumber, string cardExpiration, string cardCvv)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            Total = total;
            OrderProducts = orderProducts;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
            CardCvv = cardCvv;
        }
    }

}
