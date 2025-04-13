namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderStatus { get; set; }
    }
}
