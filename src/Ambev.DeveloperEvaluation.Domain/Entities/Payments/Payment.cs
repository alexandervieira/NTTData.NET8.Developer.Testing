using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvoluation.Core.DomainObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Payments
{
    public class Payment : BaseEntity, IAggregateRoot
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = null!;
        public decimal Amount { get; set; }

        public string CardName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string CardExpiration { get; set; } = null!;
        public string CardCvv { get; set; } = null!;

        // EF Relationship
        public Transaction Transaction { get; set; } = null!;
    }

}
