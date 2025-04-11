using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
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
}
