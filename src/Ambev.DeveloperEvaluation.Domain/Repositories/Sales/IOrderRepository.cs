using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvoluation.Core.Data;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Sales
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<PaginatedList<Order>> GetAll(int pageNumber, int pageSize, string? query);
        Task<Order?> GetById(Guid id);
        Task<IEnumerable<Order>> GetListByCustomerId(Guid CustomerId);
        Task<Order?> GetDraftOrderByCustomerId(Guid CustomerId);
        void Add(Order order);
        void Update(Order order);
        void UpdateDetach(Order order);

        Task<OrderItem?> GetItemById(Guid id);
        Task<OrderItem?> GetItemByOrder(Guid orderId, Guid productId);
        void AddItem(OrderItem orderItem);
        void UpdateItem(OrderItem orderItem);
        void RemoveItem(OrderItem orderItem);
        Task<Voucher?> GetVoucherByCode(string code);
        Task<bool> AddVoucher(Voucher voucher);
    }
}
