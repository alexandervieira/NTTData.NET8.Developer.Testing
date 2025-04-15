using Ambev.DeveloperEvoluation.Core.Messages.Commons.DomainEvents;

namespace Ambev.DeveloperEvaluation.Domain.Events.Catalog
{
    public class ProductLowStockEvent : DomainEvent
    {
        public int Quantity { get; private set; }
        public ProductLowStockEvent(Guid aggregateId, int quantity) : base(aggregateId)
        {
            Quantity = quantity;
        }
        
    }
}
