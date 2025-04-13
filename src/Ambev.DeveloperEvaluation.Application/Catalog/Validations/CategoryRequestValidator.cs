using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Validations
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(150).WithMessage("Name cannot be longer than 150 characters.");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required");
        }
                
    }
   
}
