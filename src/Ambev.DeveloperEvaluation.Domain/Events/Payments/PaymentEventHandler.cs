using Ambev.DeveloperEvaluation.Domain.Services.Payments;
using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Payments
{
    public class PaymentEventHandler : INotificationHandler<OrderStockConfirmedEvent>
    {
        private readonly IPaymentService _paymentService;

        public PaymentEventHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Handle(OrderStockConfirmedEvent message, CancellationToken cancellationToken)
        {
            var orderPayment = new OrderPayment
            {
                OrderId = message.OrderId,
                CustomerId = message.CustomerId,
                Total = message.Total,
                CardHolderName = message.CardHolderName,
                CardNumber = message.CardNumber,
                CardExpiration = message.CardExpiration,
                CardCvv = message.CardCvv
            };

            await _paymentService.ProcessOrderPayment(orderPayment);
        }
    }

}
