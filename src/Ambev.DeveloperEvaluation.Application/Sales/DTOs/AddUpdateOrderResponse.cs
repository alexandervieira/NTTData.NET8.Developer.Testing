namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class AddUpdateOrderResponse
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }        
        public List<AddUpdateOrderItemsResponse> Items { get; set; } = new List<AddUpdateOrderItemsResponse>();
        
    }
}
 