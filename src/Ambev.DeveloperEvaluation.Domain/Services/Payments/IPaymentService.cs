using Ambev.DeveloperEvaluation.Domain.Entities.Payments;
using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;

namespace Ambev.DeveloperEvaluation.Domain.Services.Payments
{
    public interface IPaymentService
    {
        Task<Transaction> ProcessOrderPayment(OrderPayment orderPayment);
    }
}
