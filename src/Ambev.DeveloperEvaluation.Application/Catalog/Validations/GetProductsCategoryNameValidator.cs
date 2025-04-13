using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Validations
{
    public class GetProductsCategoryNameValidator : AbstractValidator<GetProductsCategoryNameRequest>
    {
        public GetProductsCategoryNameValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z]+$")
                .WithMessage("Name can only contain letters.");

        }
    }
}
