using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation.Catalog
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters.");

            RuleFor(p => p.Code)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Code cannot be less than or equal to 0");

        }
    }
}
