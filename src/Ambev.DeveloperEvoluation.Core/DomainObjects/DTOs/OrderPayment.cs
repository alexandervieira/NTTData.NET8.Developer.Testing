namespace Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs
{
    public class OrderPayment
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Total { get; set; }
        public string CardHolderName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string CardExpiration { get; set; } = null!;
        public string CardCvv { get; set; } = null!;
    }

}
