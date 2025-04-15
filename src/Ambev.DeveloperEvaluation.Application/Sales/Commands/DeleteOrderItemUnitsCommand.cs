using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class DeleteOrderItemUnitsCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public DeleteOrderItemUnitsCommand(Guid customerId, Guid productId, int quantity)
        {
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteOrderItemUnitsValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteOrderItemUnitsValidation : AbstractValidator<DeleteOrderItemUnitsCommand>
    {
        public DeleteOrderItemUnitsValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid customer ID");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid product ID");
        }
    }

}
