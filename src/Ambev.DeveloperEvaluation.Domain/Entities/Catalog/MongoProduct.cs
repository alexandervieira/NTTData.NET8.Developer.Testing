using Ambev.DeveloperEvaluation.Core.DomainObjects.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Catalog
{
    public class MongoProduct : BaseEntity
    {        
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public int QuantityStock { get; set; }
        public Rating? Rating { get; set; } = null;
        public bool Active { get; set; }
        public Dimensions? Dimensions { get; set; } = null;
        public MongoCategory Category { get; set; }

        public MongoProduct() { }

        public MongoProduct(             
            Guid categoryId, 
            string title, 
            decimal price, 
            bool active, 
            string? description,
            string? image, 
            int quantityStock, 
            Rating? rating, 
            Dimensions? dimensions           
        )
        {            
            CategoryId = categoryId;
            Title = title;
            Description = description;
            Price = price;
            Image = image;
            QuantityStock = quantityStock;
            Rating = rating;
            Active = active;
            Dimensions = dimensions;           
        }
    }
}
