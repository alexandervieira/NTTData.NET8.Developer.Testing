using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class RatingValidator : AbstractValidator<Rating>
    {
        public RatingValidator()
        {
            RuleFor(d => d.Rate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Height cannot be less than or equal to 0");

            RuleFor(d => d.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Width cannot be less than or equal to 0");

        }
    }
}
