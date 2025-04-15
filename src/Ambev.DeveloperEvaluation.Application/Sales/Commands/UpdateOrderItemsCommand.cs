using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;
using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class UpdateOrderItemsCommand : Command
    {
        public Guid CustomerId { get; set; }
        public List<AddUpdateOrderItemRequest> Items { get; set; } = new List<AddUpdateOrderItemRequest>();

        public UpdateOrderItemsCommand(Guid customerId, List<AddUpdateOrderItemRequest> items)
        {
            CustomerId = customerId;
            Items = items;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderItemsValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateOrderItemsValidation : AbstractValidator<UpdateOrderItemsCommand>
    {
        public UpdateOrderItemsValidation()
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
