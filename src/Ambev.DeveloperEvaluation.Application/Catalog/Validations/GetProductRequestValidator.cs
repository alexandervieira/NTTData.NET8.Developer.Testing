using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Validations
{
    public class GetProductRequestValidator : AbstractValidator<GetProductRequest>
    {
        public GetProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
           
        }
    }
}
