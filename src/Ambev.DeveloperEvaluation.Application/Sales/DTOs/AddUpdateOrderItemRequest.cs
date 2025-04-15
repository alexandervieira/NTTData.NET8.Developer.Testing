namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs
{
    public class AddUpdateOrderItemRequest
    {
        public Guid ProductId { get; set; }        
        public int Quantity { get; set; }
      
    }
}
