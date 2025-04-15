using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class DeleteOrderItemUnitsRequest
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class DeleteOrderItemUnitsValidator : AbstractValidator<DeleteOrderItemUnitsRequest>
    {
        public DeleteOrderItemUnitsValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer ID is required.");
            
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid product ID");

            RuleFor(x => x.Quantity)
              .GreaterThan(0)
              .WithMessage("Quantity must be greater than zero.");
        }
    }
}