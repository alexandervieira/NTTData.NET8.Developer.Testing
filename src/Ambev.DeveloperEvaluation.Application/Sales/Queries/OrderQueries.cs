using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueries(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PaginatedList<CartResponse>> GetAll(int pageNumber, int pageSize, string query)
        {
            var response = await _orderRepository.GetAll(pageNumber, pageSize, query);
            if (response == null) return null;
            var cartViews = new List<CartResponse>();
            foreach (var order in response)
            {
                var cart = new CartResponse
                {
                    CustomerId = order.CustomerId,
                    TotalValue = order.TotalValue,
                    OrderId = order.Id,
                    DiscountValue = order.Discount,
                    SubTotal = order.Discount + order.TotalValue
                };

                if (order.VoucherId != null)
                {
                    cart.VoucherCode = order.Voucher.Code;
                }

                foreach (var item in order.OrderItems)
                {
                    cart.Items.Add(new CartItemResponse
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.UnitPrice * item.Quantity
                    });
                }

                cartViews.Add(cart);
            }
            return new PaginatedList<CartResponse>(cartViews, response.TotalCount, pageNumber, pageSize);
        }

        public async Task<CartResponse> GetCustomerCart(Guid customerId)
        {
            var order = await _orderRepository.GetDraftOrderByCustomerId(customerId);
            if (order == null) return null;

            var cart = new CartResponse
            {
                CustomerId = order.CustomerId,
                TotalValue = order.TotalValue,
                OrderId = order.Id,
                DiscountValue = order.Discount,
                SubTotal = order.Discount + order.TotalValue
            };

            if (order.VoucherId != null)
            {
                cart.VoucherCode = order.Voucher.Code;
            }

            foreach (var item in order.OrderItems)
            {
                cart.Items.Add(new CartItemResponse
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.UnitPrice * item.Quantity
                });
            }

            return cart;
        }

        public async Task<IEnumerable<OrderResponse>> GetCustomerOrders(Guid customerId)
        {
            var orders = await _orderRepository.GetListByCustomerId(customerId);

            orders = orders
                .Where(o => o.Status == OrderStatus.Paid || o.Status == OrderStatus.Canceled)
                .OrderByDescending(o => o.Code);

            if (!orders.Any()) return null;

            var orderViews = new List<OrderResponse>();

            foreach (var order in orders)
            {
                orderViews.Add(new OrderResponse
                {
                    Id = order.Id,
                    TotalValue = order.TotalValue,
                    OrderStatus = (int)order.Status,
                    Code = order.Code,
                    CreatedAt = order.CreatedAt
                });
            }

            return orderViews;
        }
    }

}
