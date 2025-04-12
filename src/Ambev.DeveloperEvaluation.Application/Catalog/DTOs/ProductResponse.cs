namespace Ambev.DeveloperEvaluation.Application.Catalog.DTOs
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Image { get; set; } = null!;
        public int QuantityStock { get; set; }
        public double Rate { get; set; }
        public int Count { get; set; }
        public bool Canceled { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Depth { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
    }
}
