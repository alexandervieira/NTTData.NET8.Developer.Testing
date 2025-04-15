using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class DeleteOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }

        public DeleteOrderItemCommand(Guid customerId, Guid productId)
        {
            CustomerId = customerId;
            ProductId = productId;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteOrderItemValidation : AbstractValidator<DeleteOrderItemCommand>
    {
        public DeleteOrderItemValidation()
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
