using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using FluentValidation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales
{
    public class Voucher : BaseEntity
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

        // EF Core navigation property
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ValidationResult ValidateIfApplicable()
        {
            return new ApplicableVoucherValidation().Validate(this);
        }
    }

    public class ApplicableVoucherValidation : AbstractValidator<Voucher>
    {
        public ApplicableVoucherValidation()
        {
            RuleFor(v => v.ExpirationDate)
                .Must(BeValidExpirationDate)
                .WithMessage("This voucher has expired.");

            RuleFor(v => v.Active)
                .Equal(true)
                .WithMessage("This voucher is no longer valid.");

            RuleFor(v => v.Used)
                .Equal(false)
                .WithMessage("This voucher has already been used.");

            RuleFor(v => v.Quantity)
                .GreaterThan(0)
                .WithMessage("This voucher is no longer available.");
        }

        protected static bool BeValidExpirationDate(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }

}
