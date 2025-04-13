using Ambev.DeveloperEvaluation.Domain.Entities.Payments;

namespace Ambev.DeveloperEvaluation.Domain.Services.Payments
{
    public interface ICreditCardPaymentFacade
    {
        Transaction ProcessPayment(Order order, Payment payment);
    }
}
