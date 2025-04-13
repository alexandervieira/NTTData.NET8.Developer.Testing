using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation.Catalog;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvoluation.Core.DomainObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Catalog
{
    public class Product : BaseEntity, IAggregateRoot 
    {
        public Guid CategoryId { get; private set; }
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public decimal Price { get; private set; }
        public string? Image { get; private set; }
        public int QuantityStock { get; private set; }
        public Rating? Rating { get; private set; }
        public bool Canceled { get; private set; }
        public Dimensions? Dimensions { get; private set; }
        public virtual Category Category { get; set; } = null!;
        public DateTime CreateDate { get; set; }

        protected Product() { }

        public Product(string title, decimal price, bool canceled)
            : this(title, "Minha descrição", canceled, price, Guid.NewGuid(), string.Empty, new Rating(0,0), new Dimensions(0, 0, 0), DateTime.UtcNow) { }

        public Product(string title, string description, bool canceled,
                       decimal price, Guid categoryId, string image, Rating rating,
                       Dimensions dimensions, DateTime createDate)
        {
            Title = title;
            Description = description;
            Canceled = canceled;
            Price = price;
            CategoryId = categoryId;
            Image = image;
            Rating = rating;
            Dimensions = dimensions;
            CreateDate = createDate;            
        }

        public void Activate() => Canceled = false;

        public void Cancel() => Canceled = true;

        public void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            Category = category;
            CategoryId = category.Id;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            Description = description;
        }

        public void DebitStock(int quantity)
        {
            if (quantity < 0) quantity *= -1;
            if (!HasStock(quantity)) throw new DomainException("Estoque insuficiente");
            QuantityStock -= quantity;
        }

        public void ReplenishStock(int quantity)
        {
            QuantityStock += quantity;
        }

        public bool HasStock(int quantity)
        {
            return QuantityStock >= quantity;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new ProductValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
