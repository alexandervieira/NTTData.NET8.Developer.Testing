namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs
{
    public class CartResponse
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalValue { get; set; }
        public decimal DiscountValue { get; set; }
        public string VoucherCode { get; set; } = null!;
        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
        public CartPaymentResponse Payment { get; set; } = null!;
    }
}
