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
        public string VoucherCode { get; set; } = null!;
        public List<CartItemRequest> Items { get; set; } = new List<CartItemRequest>();
        public CartPaymentRequest Payment { get; set; } = null!;
    }

    public class CartRequestValidator : AbstractValidator<CartRequest>
    {
        public CartRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer ID is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required in the cart.");
        }
    }
}