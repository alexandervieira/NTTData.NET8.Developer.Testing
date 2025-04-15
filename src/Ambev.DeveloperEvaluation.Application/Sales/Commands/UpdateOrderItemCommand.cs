using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class UpdateOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public UpdateOrderItemCommand(Guid customerId, Guid productId, int quantity)
        {
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateOrderItemValidation : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client ID");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid product ID");

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage("The minimum quantity for an item is 1");

            RuleFor(c => c.Quantity)
                .LessThan(20)
                .WithMessage("The maximum quantity for an item is 20");
        }
    }

}
