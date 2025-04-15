using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class AddUpdateOrderRequest
    {
        public Guid CustomerId { get; set; }
        public List<AddUpdateOrderItemRequest> Items { get; set; } = new List<AddUpdateOrderItemRequest>();
    }

    public class AddUpdateOrderRequestValidator : AbstractValidator<AddUpdateOrderRequest>
    {
        public AddUpdateOrderRequestValidator()
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