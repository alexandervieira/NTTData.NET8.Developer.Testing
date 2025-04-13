using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class AddOrderItemCommand : Command
    {
        public Guid ClientId { get; private set; }
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public AddOrderItemCommand(Guid clientId,
                                   Guid productId,
                                   string name,
                                   int quantity,
                                   decimal unitPrice)
        {
            ClientId = clientId;
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {
        public AddOrderItemValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client ID");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid product ID");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("The product name was not provided");

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage("The minimum quantity for an item is 1");

            RuleFor(c => c.Quantity)
                .LessThan(15)
                .WithMessage("The maximum quantity for an item is 15");

            RuleFor(c => c.UnitPrice)
                .GreaterThan(0)
                .WithMessage("The unit price must be greater than 0");
        }
    }

}
