using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Core.DomainObjects.ValueObjects
{
    public class Rating
    {
        public double Rate { get; private set; }
        public int Count { get; private set; }

        public Rating(double rate, int count)
        {
            Rate = rate;
            Count = count;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new RatingValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }

    public class RatingValidator : AbstractValidator<Rating>
    {
        public RatingValidator()
        {
            RuleFor(r => r.Rate)
                .InclusiveBetween(0, 5)
                .WithMessage("Rate must be between 0 and 5");

            RuleFor(r => r.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Count cannot be less than 0");
        }
    }
}