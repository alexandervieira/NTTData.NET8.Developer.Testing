using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Catalog.DTOs
{
    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool Canceled { get; set; }
        public string? Image { get; set; }
        public int QuantityStock { get; set; }
        public Rating Rating { get; set; } = null!;        
        public Dimensions Dimensions { get; set; } = null!;

    }
}
