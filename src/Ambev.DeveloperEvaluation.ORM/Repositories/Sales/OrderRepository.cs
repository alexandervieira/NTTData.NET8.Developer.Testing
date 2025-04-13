using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using Ambev.DeveloperEvoluation.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Sales
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DefaultContext _context;

        public OrderRepository(DefaultContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Order?> GetById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetListByCustomerId(Guid customerId)
        {
            return await _context
                            .Orders
                            .AsNoTracking()
                            .Where(p => p.CustomerId == customerId)
                            .ToListAsync();
        }

        public async Task<Order?> GetDraftOrderByCustomerId(Guid customerId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.CustomerId == customerId && p.Status == OrderStatus.Draft);
            if (order == null) return null;

            await _context.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();

            if (order.VoucherId != null)
            {
                await _context.Entry(order)
                    .Reference(i => i.Voucher).LoadAsync();
            }

            return order;
        }

        public void Add(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            _context.Orders.Add(order);
        }

        public void Update(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            _context.Orders.Update(order);
        }

        public async Task<OrderItem?> GetItemById(Guid id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItem?> GetItemByOrder(Guid orderId, Guid productId)
        {
            return await _context
                .OrderItems
                .FirstOrDefaultAsync(p => 
                    p.ProductId == productId && p.OrderId == orderId
                  );
        }

        public void AddItem(OrderItem orderItem)
        {
            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            _context.OrderItems.Add(orderItem);
        }

        public void UpdateItem(OrderItem orderItem)
        {
            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            _context.OrderItems.Update(orderItem);
        }

        public void RemoveItem(OrderItem orderItem)
        {
            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            _context.OrderItems.Remove(orderItem);
        }

        public async Task<Voucher?> GetVoucherByCode(string code)
        {
            return await (from v in _context.Vouchers
                          where v.Code == code
                          select v
                          ).FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
