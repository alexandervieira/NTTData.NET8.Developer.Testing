using Ambev.DeveloperEvoluation.Core.Messages;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class CancelOrderProcessingAndRestockCommand : Command
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }

        public CancelOrderProcessingAndRestockCommand(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }
    }

}
