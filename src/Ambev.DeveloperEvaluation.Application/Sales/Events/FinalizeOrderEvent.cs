using Ambev.DeveloperEvoluation.Core.Messages;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class FinalizeOrderEvent : Event
    {
        public Guid OrderId { get; private set; }

        public FinalizeOrderEvent(Guid orderId)
        {
            OrderId = orderId;
            AggregateId = orderId;
        }
    }

}
