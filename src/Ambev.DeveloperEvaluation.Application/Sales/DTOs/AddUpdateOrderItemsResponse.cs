namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class AddUpdateOrderItemsResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        
    }
}
