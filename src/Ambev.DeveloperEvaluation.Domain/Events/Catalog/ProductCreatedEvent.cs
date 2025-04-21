using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.DomainEvents;

namespace Ambev.DeveloperEvoluation.Domain.Events.Catalog;
public class ProductCreatedEvent : DomainEvent
{
    public Product Product { get; set; }

    public ProductCreatedEvent(Product product) : base(product.Id)
    {
        Product = product;
    }
}
