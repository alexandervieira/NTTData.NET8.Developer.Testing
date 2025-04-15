using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries
{
    public interface IOrderQueries
    {
        Task<PaginatedList<CartResponse>> GetAll(int pageNumber, int pageSize, string query);
        Task<CartResponse> GetCustomerCart(Guid customerId);
        Task<IEnumerable<OrderResponse>> GetCustomerOrders(Guid customerId);        
    }

}
