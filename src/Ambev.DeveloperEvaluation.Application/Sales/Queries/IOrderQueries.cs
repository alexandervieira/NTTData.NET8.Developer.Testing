using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries
{
    public interface IOrderQueries
    {
        Task<CartResponse> GetCustomerCart(Guid customerId);
        Task<IEnumerable<OrderResponse>> GetCustomerOrders(Guid customerId);
    }

}
