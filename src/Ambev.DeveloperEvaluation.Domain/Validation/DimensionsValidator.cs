using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class DimensionsValidator : AbstractValidator<Dimensions>
    {
        public DimensionsValidator()
        {
            RuleFor(d => d.Height)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Height cannot be less than or equal to 0");

            RuleFor(d => d.Width)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Width cannot be less than or equal to 0");

            RuleFor(d => d.Depth)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Depth cannot be less than or equal to 0");

        }
    }
}
