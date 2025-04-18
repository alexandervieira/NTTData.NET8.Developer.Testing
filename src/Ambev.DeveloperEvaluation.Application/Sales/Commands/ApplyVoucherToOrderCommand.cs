﻿using Ambev.DeveloperEvoluation.Core.Messages;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class ApplyVoucherToOrderCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public string VoucherCode { get; private set; }

        public ApplyVoucherToOrderCommand(Guid customerId, string voucherCode)
        {
            CustomerId = customerId;
            VoucherCode = voucherCode;
        }

        public override bool IsValid()
        {
            ValidationResult = new ApplyVoucherToOrderValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ApplyVoucherToOrderValidation : AbstractValidator<ApplyVoucherToOrderCommand>
    {
        public ApplyVoucherToOrderValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client ID");

            RuleFor(c => c.VoucherCode)
                .NotEmpty()
                .WithMessage("The voucher code cannot be empty");
        }
    }

}
