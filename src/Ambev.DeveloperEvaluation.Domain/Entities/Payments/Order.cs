namespace Ambev.DeveloperEvaluation.Domain.Entities.Payments
{
    public class Order
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
