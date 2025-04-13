using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = null!;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        // EF Core navigation property
        public Order Order { get; set; } = null!;

        public OrderItem(Guid productId,
                         string productName,
                         int quantity,
                         decimal unitPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        protected OrderItem() { }

        internal void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public decimal CalculateValue()
        {
            return Quantity * UnitPrice;
        }

        public void AddUnits(int units)
        {
            Quantity += units;
        }

        public void UpdateUnits(int units)
        {
            Quantity = units;
        }

        public override bool IsValid()
        {
            return true;
        }
    }

}
