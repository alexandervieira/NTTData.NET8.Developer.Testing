using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using Ambev.DeveloperEvoluation.Core.DomainObjects;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public int Code { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool IsVoucherUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set; }        
        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        // EF Core navigation property
        public Voucher Voucher { get; private set; } = null!;

        public Order(Guid customerId,
                     bool isVoucherUsed,
                     decimal discount,
                     decimal totalValue)
        {
            CustomerId = customerId;
            IsVoucherUsed = isVoucherUsed;
            Discount = discount;
            TotalValue = totalValue;
            _orderItems = new List<OrderItem>();
        }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var validationResult = voucher.ValidateIfApplicable();
            if (!validationResult.IsValid) return validationResult;

            if (IsVoucherUsed && VoucherId == voucher.Id)
            {
                validationResult.Errors.Add(new ValidationFailure("", "This voucher has already been applied to the order."));
                return validationResult;
            }

            Voucher = voucher;
            IsVoucherUsed = true;
            VoucherId = voucher.Id;
            CalculateOrderValue();

            return validationResult;
        }

        public void CalculateOrderValue()
        {
            TotalValue = OrderItems.Sum(item => item.CalculateValue());
            CalculateTotalDiscount();
        }

        public void CalculateTotalDiscount()
        {
            if (!IsVoucherUsed) return;

            decimal discount = 0;
            var value = TotalValue;

            if (Voucher.DiscountVoucherType == DiscountVoucherType.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    discount = (value * Voucher.Percentage.Value) / 100;
                    value -= discount;
                }
            }
            else
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                    value -= discount;
                }
            }

            TotalValue = value < 0 ? 0 : value;
            Discount = discount;
        }

        public bool HasOrderItem(OrderItem item)
        {
            return _orderItems.Any(p => p.ProductId == item.ProductId);
        }

        public void AddItem(OrderItem item)
        {
            if (!item.IsValid()) return;

            item.AssociateOrder(Id);

            if (HasOrderItem(item))
            {
                var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (existingItem == null) throw new DomainException("The item does not belong to the order.");
                existingItem.AddUnits(item.Quantity);
                item = existingItem;

                _orderItems.Remove(existingItem);
            }

            item.CalculateValue();
            _orderItems.Add(item);

            CalculateOrderValue();
        }

        public void RemoveItem(OrderItem item)
        {
            if (!item.IsValid()) return;

            var existingItem = OrderItems.FirstOrDefault(p => p.ProductId == item.ProductId);

            if (existingItem == null) throw new DomainException("The item does not belong to the order.");
            _orderItems.Remove(existingItem);

            CalculateOrderValue();
        }

        public void UpdateItem(OrderItem item)
        {
            if (!item.IsValid()) return;
            item.AssociateOrder(Id);

            var existingItem = OrderItems.FirstOrDefault(p => p.ProductId == item.ProductId);

            if (existingItem == null) throw new DomainException("The item does not belong to the order.");

            _orderItems.Remove(existingItem);
            _orderItems.Add(item);

            CalculateOrderValue();
        }

        public void UpdateUnits(OrderItem item, int units)
        {
            item.UpdateUnits(units);
            UpdateItem(item);
        }

        public void SetAsDraft()
        {
            Status = OrderStatus.Draft;
        }

        public void StartOrder()
        {
            Status = OrderStatus.Started;
        }

        public void CompleteOrder()
        {
            Status = OrderStatus.Paid;
        }

        public void CancelOrder()
        {
            Status = OrderStatus.Canceled;
        }

        public static class OrderFactory
        {
            public static Order NewDraftOrder(Guid customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId,
                };

                order.SetAsDraft();
                return order;
            }
        }
    }

}
