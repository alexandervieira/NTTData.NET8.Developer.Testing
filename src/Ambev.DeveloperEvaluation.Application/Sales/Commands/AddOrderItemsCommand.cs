using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;
using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class AddOrderItemsCommand : Command
    {
        public Guid CustomerId { get; set; }
        public List<AddUpdateOrderItemRequest> Items { get; set; } = new List<AddUpdateOrderItemRequest>();

        public AddOrderItemsCommand(Guid customerId, List<AddUpdateOrderItemRequest> items)
        {
            CustomerId = customerId;
            Items = items;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemsCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddOrderItemsCommandValidator : AbstractValidator<AddOrderItemsCommand>
    {
        public AddOrderItemsCommandValidator()
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
