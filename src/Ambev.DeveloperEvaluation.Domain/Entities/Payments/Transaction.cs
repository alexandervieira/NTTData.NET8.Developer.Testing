using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums.Payments;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Payments
{
    public class Transaction : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Total { get; set; }
        public TransactionStatus TransactionStatus { get; set; }

        // EF Relationship
        public Payment Payment { get; set; } = null!;
    }

}
