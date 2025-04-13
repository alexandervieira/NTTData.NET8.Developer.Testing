using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Catalog.DTOs
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } = null!;
        public int QuantityStock { get; set; }
        public Rating Rating { get; set; } = null!;
        public Dimensions Dimensions { get; set; } = null!;
        //public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
    }
}
