using Ambev.DeveloperEvaluation.Domain.Entities.Payments;
using Ambev.DeveloperEvaluation.Domain.Enums.Payments;
using Ambev.DeveloperEvaluation.Domain.Repositories.Payments;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.Notifications;

namespace Ambev.DeveloperEvaluation.Domain.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly ICreditCardPaymentFacade _creditCardPaymentFacade;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PaymentService(ICreditCardPaymentFacade creditCardPaymentFacade,
                              IPaymentRepository paymentRepository,
                              IMediatorHandler mediatorHandler)
        {
            _creditCardPaymentFacade = creditCardPaymentFacade;
            _paymentRepository = paymentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<Transaction> ProcessOrderPayment(OrderPayment orderPayment)
        {
            var order = new Order
            {
                Id = orderPayment.OrderId,
                Value = orderPayment.Total
            };

            var payment = new Payment
            {
                Amount = orderPayment.Total,
                CardName = orderPayment.CardHolderName,
                CardNumber = orderPayment.CardNumber,
                CardExpiration = orderPayment.CardExpiration,
                CardCvv = orderPayment.CardCvv,
                OrderId = orderPayment.OrderId
            };

            var transaction = _creditCardPaymentFacade.ProcessPayment(order, payment);
            if (transaction == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("payment", "The payment was not processed"));
#pragma warning disable CS8603 // Possível retorno de referência nula.
                return null;
#pragma warning restore CS8603 // Possível retorno de referência nula.
            }

            if (transaction.TransactionStatus == TransactionStatus.Paid)
            {
                payment.AddEvent(new OrderPaymentCompletedEvent(order.Id, orderPayment.CustomerId, transaction.PaymentId, transaction.Id, order.Value));

                payment.Status = TransactionStatus.Paid.ToString();
                _paymentRepository.Add(payment);
                _paymentRepository.AddTransaction(transaction);

                await _paymentRepository.UnitOfWork.CommitAsync();

#pragma warning disable CS8619 // A anulabilidade de tipos de referência no valor não corresponde ao tipo de destino.
                return transaction;
#pragma warning restore CS8619 // A anulabilidade de tipos de referência no valor não corresponde ao tipo de destino.
            }

            await _mediatorHandler.PublishNotification(new DomainNotification("payment", "The payment was declined by the operator"));
            await _mediatorHandler.PublishEvent(new OrderPaymentRejectedEvent(order.Id, orderPayment.CustomerId, transaction.PaymentId, transaction.Id, order.Value));

#pragma warning disable CS8619 // A anulabilidade de tipos de referência no valor não corresponde ao tipo de destino.
            return transaction;
#pragma warning restore CS8619 // A anulabilidade de tipos de referência no valor não corresponde ao tipo de destino.
        }
    }

}
