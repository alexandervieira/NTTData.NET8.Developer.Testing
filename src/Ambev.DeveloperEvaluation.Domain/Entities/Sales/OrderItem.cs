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
        public decimal Discount { get; private set; } // Propriedade Discount adicionada

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
            return Quantity * UnitPrice * (1 - Discount); // Considera o desconto ao calcular o valor
        }

        public void DebitUnits(int units)
        {
            Quantity -= units;
        }

        public void AddUnits(int units)
        {
            Quantity += units;
        }

        public void UpdateUnits(int units)
        {
            Quantity = units;
        }

        public void ApplyDiscount(decimal discountPercentage)
        {
            Discount = discountPercentage; // Armazena o valor do desconto
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
