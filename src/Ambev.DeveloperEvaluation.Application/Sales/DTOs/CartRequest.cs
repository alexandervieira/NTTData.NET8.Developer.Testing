using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class CartRequest
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalValue { get; set; }
        public decimal DiscountValue { get; set; }
        public string? VoucherCode { get; set; }
        public List<CartItemRequest> Items { get; set; } = new List<CartItemRequest>();
        public CartPaymentRequest Payment { get; set; } = null!;
    }

    public class CartRequestValidator : AbstractValidator<CartRequest>
    {
        public CartRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("Order ID is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer ID is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required in the cart.");           

            RuleFor(x => x.VoucherCode)
                .Matches(@"^[a-zA-Z0-9]*$")
                .WithMessage("Voucher code must be alphanumeric.")
                .When(x => !string.IsNullOrEmpty(x.VoucherCode))
                .WithMessage("Voucher code must be empty if not used.");

            RuleFor(x => x.DiscountValue)
                .NotEmpty()
                .When(x => x.DiscountValue > 0)
                .WithMessage("Discount value must be greater than zero if applied.");

            RuleFor(x => x.Payment)
           .NotEmpty()
           .WithMessage("Payment information is required.")
           .ChildRules(payment =>
           {
               payment.RuleFor(p => p.CardName)
                   .NotEmpty()
                   .WithMessage("Card name is required.");

               payment.RuleFor(p => p.CardNumber)
                   .NotEmpty()
                   .WithMessage("Card number is required.")
                   .CreditCard()
                   .WithMessage("Card number is invalid.");

               payment.RuleFor(p => p.CardExpiration)
                   .NotEmpty()
                   .WithMessage("Card expiration date is required.")
                   .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$")
                   .WithMessage("Card expiration date must be in MM/YY format.");

               payment.RuleFor(p => p.CardCvv)
                   .NotEmpty()
                   .WithMessage("Card CVV is required.")
                   .Matches(@"^\d{3,4}$")
                   .WithMessage("Card CVV must be 3 or 4 digits.");
           });

        }
    }
}