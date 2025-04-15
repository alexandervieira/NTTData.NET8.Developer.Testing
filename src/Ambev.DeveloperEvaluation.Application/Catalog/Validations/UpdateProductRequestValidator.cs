using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Validations
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            //RuleFor(x => x.CategoryId)
            //    .NotEmpty()
            //    .WithMessage("CategoryId is required");

            RuleFor(p => p.Title)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(150).WithMessage("Title cannot be longer than 150 characters.");

            RuleFor(p => p.Description)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Description must be at least 3 characters long.")
                .MaximumLength(250).WithMessage("Description cannot be longer than 250 characters.");

            RuleFor(p => p.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price cannot be less than or equal to 0");
        }
    }
}
