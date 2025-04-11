using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
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
}
