using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation.Catalog;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Catalog
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; } = null!;
        public int Code { get; private set; }

        // EF Relation
        public virtual ICollection<Product>? Products { get; set; }

        protected Category() { }

        public Category(string name, int code)
        {
            Name = name;
            Code = code;

            //Validate();
        }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }

        public ValidationResultDetail Validate()
        {
            var validator = new CategoryValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
