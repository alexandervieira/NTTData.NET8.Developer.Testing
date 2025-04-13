using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvoluation.Core.Data;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Sales
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetById(Guid id);
        Task<IEnumerable<Order>> GetListByCustomerId(Guid CustomerId);
        Task<Order?> GetDraftOrderByCustomerId(Guid CustomerId);
        void Add(Order order);
        void Update(Order order);

        Task<OrderItem?> GetItemById(Guid id);
        Task<OrderItem?> GetItemByOrder(Guid orderId, Guid productId);
        void AddItem(OrderItem orderItem);
        void UpdateItem(OrderItem orderItem);
        void RemoveItem(OrderItem orderItem);

        Task<Voucher?> GetVoucherByCode(string code);
    }
}
