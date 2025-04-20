using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class GenereateVoucherOrderCommand : Command
    {
        public string Code { get; set; } = null!;
        public decimal? Percentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public int Quantity { get; set; }
        public DiscountVoucherType DiscountVoucherType { get; set; }
        public DateTime? UsageDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Active { get; set; }
        public bool Used { get; set; }

        public GenereateVoucherOrderCommand(string code, decimal? percentage, decimal? discountValue, int quantity,
            DiscountVoucherType discountVoucherType, DateTime? usageDate, DateTime expirationDate, bool active, bool used)
        {
            Code = code;
            Percentage = percentage;
            DiscountValue = discountValue;
            Quantity = quantity;
            DiscountVoucherType = discountVoucherType;
            UsageDate = usageDate;
            ExpirationDate = expirationDate;
            Active = active;
            Used = used;
        }

        public override bool IsValid()
        {
            ValidationResult = new GenerateVoucherRequestValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class GenerateVoucherRequestValidator : AbstractValidator<GenereateVoucherOrderCommand>
    {
        public GenerateVoucherRequestValidator()
        {
            RuleFor(v => v.Code)
                .NotEmpty()
                .WithMessage("The voucher code cannot be empty")
                .Matches(@"^[a-zA-Z0-9]*$")
                .WithMessage("Voucher code must be alphanumeric.");

            RuleFor(v => v.Percentage)
                .GreaterThan(0)
                .When(v => v.DiscountVoucherType == DiscountVoucherType.Percentage)
                .WithMessage("Percentage must be greater than 0 when the discount type is percentage.");

            RuleFor(v => v.DiscountValue)
                .GreaterThan(0)
                .When(v => v.DiscountVoucherType == DiscountVoucherType.Value)
                .WithMessage("Discount value must be greater than 0 when the discount type is value.");

            RuleFor(v => v.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(v => v.ExpirationDate)
                .NotEmpty()
                .WithMessage("Expiration date is required.")
                .Must(BeValidExpirationDate)
                .WithMessage("Expiration date must be in the future.");

            RuleFor(v => v.UsageDate)
                .Must(BeValidUsageDate)
                .WithMessage("Usage date must be in the future.")
                .When(v => v.UsageDate.HasValue)
                .WithMessage("Usage date must be in the future if provided.");

            RuleFor(v => v.Active)
                .NotNull()
                .WithMessage("Active status is required.");

            RuleFor(v => v.Used)
                .NotNull()
                .WithMessage("Used status is required.");

            RuleFor(v => v.DiscountVoucherType)
                .IsInEnum()
                .WithMessage("Invalid discount voucher type.");

        }

        private static bool BeValidUsageDate(DateTime? usageDate)
        {
            if (usageDate == null)
                return true;

            var time = usageDate.Value;
            return time >= DateTime.Now;
        }

        private static bool BeValidExpirationDate(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }
}
