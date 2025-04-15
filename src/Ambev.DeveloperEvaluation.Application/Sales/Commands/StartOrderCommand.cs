using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class StartOrderCommand : Command
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public decimal Total { get; private set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpiration { get; private set; }
        public string CardCvv { get; private set; }

        public StartOrderCommand(Guid orderId, Guid customerId, decimal total, string cardName, string cardNumber, string cardExpiration, string cardCvv)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Total = total;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
            CardCvv = cardCvv;
        }

        public override bool IsValid()
        {
            ValidationResult = new StartOrderValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class StartOrderValidation : AbstractValidator<StartOrderCommand>
    {
        public StartOrderValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client ID");

            RuleFor(c => c.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid order ID");

            RuleFor(c => c.CardName)
                .NotEmpty()
                .WithMessage("Cardholder name is required");

            RuleFor(c => c.CardNumber)
                .CreditCard()
                .WithMessage("Invalid credit card number");

            RuleFor(c => c.CardExpiration)
                .NotEmpty()
                .WithMessage("Card expiration date is required");

            RuleFor(c => c.CardCvv)
                .Length(3, 4)
                .WithMessage("CVV must be 3 or 4 digits long");
        }
    }

}
