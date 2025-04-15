using Ambev.DeveloperEvaluation.Domain.Entities.Payments;
using Ambev.DeveloperEvaluation.Domain.Repositories.Payments;
using Ambev.DeveloperEvoluation.Core.Data;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Payments
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DefaultContext _context;

        public PaymentRepository(DefaultContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
