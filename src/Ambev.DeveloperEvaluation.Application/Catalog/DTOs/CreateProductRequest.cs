using Ambev.DeveloperEvaluation.Core.DomainObjects.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Catalog.DTOs
{
    public class CreateProductRequest
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public int QuantityStock { get; set; }        
        public bool Active { get; set; }
        public Rating Rating { get; set; } = null!;
        public Dimensions Dimensions { get; set; } = null!;        

    }
}
