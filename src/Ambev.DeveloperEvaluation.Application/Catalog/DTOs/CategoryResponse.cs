namespace Ambev.DeveloperEvaluation.Application.Catalog.DTOs
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Code { get; set; }
    }
}
