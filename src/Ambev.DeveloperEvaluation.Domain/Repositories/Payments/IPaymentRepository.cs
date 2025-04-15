using Ambev.DeveloperEvaluation.Domain.Entities.Payments;
using Ambev.DeveloperEvoluation.Core.Data;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Payments
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void Add(Payment payment);
        void AddTransaction(Transaction transaction);
    }
}
