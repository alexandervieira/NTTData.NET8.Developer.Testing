using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class VoucherRequest
    {
        public string VoucherCode { get; set; } = null!;

        public VoucherRequest(string voucherCode)
        {
            VoucherCode = voucherCode;
        }

    }

    public class VoucherRequestValidator : AbstractValidator<VoucherRequest>
    {
        public VoucherRequestValidator()
        {
            RuleFor(x => x.VoucherCode)
                .NotEmpty()
                .WithMessage("Voucher code is required.");
        }
    }
}
