using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class GenerateVoucherRequest
    {
        public string Code { get; set; } = null!;
        public decimal? Percentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public int Quantity { get; set; }
        public DiscountVoucherType DiscountVoucherType { get; set; }
        public DateTime? UsageDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Active { get; set; }
        public bool Used { get; set; }

        public GenerateVoucherRequest(string code, decimal? percentage, decimal? discountValue, int quantity,
            DiscountVoucherType discountVoucherType, DateTime? usageDate, DateTime expirationDate, bool active, bool used)
        {
            Code = code;
            Percentage = percentage;
            DiscountValue = discountValue;
            Quantity = quantity;
            DiscountVoucherType = discountVoucherType;
            UsageDate = usageDate;
            ExpirationDate = expirationDate;
            Active = active;
            Used = used;
        }
    }

}
