namespace Ambev.DeveloperEvaluation.Domain.Entities.Payments
{
    public class Product
    {
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Value { get; set; }
    }
}
