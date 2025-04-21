using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Core.DomainObjects.ValueObjects
{
    public class Dimensions
    {
        public double Height { get; private set; }
        public double Width { get; private set; }
        public double Depth { get; private set; }

        public Dimensions(double height, double width, double depth)
        {
            Height = height;
            Width = width;
            Depth = depth;
        }

        public string DescricaoFormatada()
        {
            return $"LxAxP: {Width} x {Height} x {Depth}";
        }

        public override string ToString()
        {
            return DescricaoFormatada();
        }
        public ValidationResultDetail Validate()
        {
            var validator = new DimensionsValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }

    }

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
