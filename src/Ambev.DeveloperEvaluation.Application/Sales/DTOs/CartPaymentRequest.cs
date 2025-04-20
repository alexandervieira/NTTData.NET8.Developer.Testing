using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class CartPaymentRequest
    {
        public string CardName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string CardExpiration { get; set; } = null!;
        public string CardCvv { get; set; } = null!;
    }

    public class CartPaymentRequestValidator : AbstractValidator<CartPaymentRequest>
    {
        public CartPaymentRequestValidator()
        {
            RuleFor(x => x.CardName)
            .NotEmpty()
            .WithMessage("Card name is required.");

            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .WithMessage("Card number is required.")
                .CreditCard()
                .WithMessage("Card number is invalid.");

            RuleFor(x => x.CardExpiration)
                .NotEmpty()
                .WithMessage("Card expiration date is required.")
                .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$")
                .WithMessage("Card expiration date must be in MM/YY format.");

            RuleFor(x => x.CardCvv)
                .NotEmpty()
                .WithMessage("Card CVV is required.")
                .Matches(@"^\d{3,4}$")
                .WithMessage("Card CVV must be 3 or 4 digits.");
        }
    }
}
