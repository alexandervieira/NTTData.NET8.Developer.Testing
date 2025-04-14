using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs
{
    public class CartRequest
    {
        public CartPaymentResponse Payment { get; set; } = null!;

        public CartRequest(CartPaymentResponse payment)
        {
            Payment = payment;
        }
    }

    public class CartRequestValidator : AbstractValidator<CartRequest>
    {
        public CartRequestValidator()
        {            

            RuleFor(x => x.Payment)
                .NotNull()
                .WithMessage("Payment information is required.");
        }
    }
}
