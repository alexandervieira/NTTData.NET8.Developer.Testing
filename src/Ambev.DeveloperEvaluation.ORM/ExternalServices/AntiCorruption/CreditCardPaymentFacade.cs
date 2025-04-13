using Ambev.DeveloperEvaluation.Domain.Entities.Payments;
using Ambev.DeveloperEvaluation.Domain.Enums.Payments;
using Ambev.DeveloperEvaluation.Domain.Services.Payments;
using Ambev.DeveloperEvaluation.ORM.ExternalServices.AntiCorruption.Interfaces;

namespace Ambev.DeveloperEvaluation.ORM.ExternalServices.AntiCorruption
{
    public class CreditCardPaymentFacade : ICreditCardPaymentFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IPaymentConfigurationManager _configManager;

        public CreditCardPaymentFacade(IPayPalGateway payPalGateway, 
                                       IPaymentConfigurationManager configManager)
        {
            _payPalGateway = payPalGateway;
            _configManager = configManager;
        }

        public Transaction ProcessPayment(Order order, Payment payment)
        {
            var apiKey = _configManager.GetValue("apiKey");
            var encryptionKey = _configManager.GetValue("encryptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encryptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, payment.CardNumber);

            var paymentResult = _payPalGateway.CommitTransaction(cardHashKey, order.Id.ToString(), payment.Amount);

            // TODO: The payment gateway should return the transaction object
            var transaction = new Transaction
            {
                OrderId = order.Id,
                Total = order.Value,
                PaymentId = payment.Id
            };

            if (paymentResult)
            {
                transaction.TransactionStatus = TransactionStatus.Paid;
                return transaction;
            }

            transaction.TransactionStatus = TransactionStatus.Declined;
            return transaction;
        }
    }

}
