using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class ItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public ItemRequest(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }

    public class ItemRequestValidator : AbstractValidator<ItemRequest>
    {
        public ItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}
