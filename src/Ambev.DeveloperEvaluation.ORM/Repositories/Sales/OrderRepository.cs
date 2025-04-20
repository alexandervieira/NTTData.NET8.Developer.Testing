using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using Ambev.DeveloperEvoluation.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Sales
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        //private readonly DefaultContext _context;

        public OrderRepository(DefaultContext context) : base(context)
        {
            //_context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => (DefaultContext)_context;

        public async Task<Order?> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<PaginatedList<Order>> GetAll(int pageNumber, int pageSize, string query)
        {
            string newquery = query != null ? query.ToLower() : string.Empty;
            var source = _dbSet
                .AsNoTrackingWithIdentityResolution()
                .Include(o => o.OrderItems)
                .Where(o => o.OrderItems.Any(oi => 
                            EF.Functions.Like(oi.ProductName.ToLower(), $"%{newquery}%")))
                .OrderBy(o => o.OrderItems.FirstOrDefault(oi => 
                            EF.Functions.Like(oi.ProductName.ToLower(), $"%{newquery}%")).ProductName)
                .AsQueryable();

            return await PaginatedList<Order>.CreateAsync(source, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Order>> GetListByCustomerId(Guid customerId)
        {
            return await _dbSet.AsNoTracking()
                               .Where(p => p.CustomerId == customerId)
                               .ToListAsync();
        }

        public async Task<Order?> GetDraftOrderByCustomerId(Guid customerId)
        {
            var order = await _dbSet.FirstOrDefaultAsync(p => 
                                        p.CustomerId == customerId && p.Status == OrderStatus.Draft);
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
            _dbSet.Add(order);
        }

        public void Update(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            _dbSet.Update(order);
        }

        public void UpdateDetach(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            Update(order);
        }

        public async Task<OrderItem?> GetItemById(Guid id)
        {
            return await _context.Set<OrderItem>().FindAsync(id);
        }

        public async Task<OrderItem?> GetItemByOrder(Guid orderId, Guid productId)
        {
            return await _context.Set<OrderItem>()
                .FirstOrDefaultAsync(p => 
                    p.ProductId == productId && p.OrderId == orderId
                  );
        }

        public void AddItem(OrderItem orderItem)
        {
            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            _context.Set<OrderItem>().Add(orderItem);
        }

        public void UpdateItem(OrderItem orderItem)
        {
            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            _context.Set<OrderItem>().Update(orderItem);
        }

        public void RemoveItem(OrderItem orderItem)
        {
            if (orderItem == null) throw new ArgumentNullException(nameof(orderItem));
            _context.Set<OrderItem>().Remove(orderItem);
        }

        public async Task<Voucher?> GetVoucherByCode(string code)
        {
            return await (from v in _context.Set<Voucher>()
                          where v.Code == code
                          select v
                          ).FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<bool> AddVoucher(Voucher voucher)
        {
            await _context.Set<Voucher>().AddAsync(voucher);
            return voucher != null;
        }
    }

}
