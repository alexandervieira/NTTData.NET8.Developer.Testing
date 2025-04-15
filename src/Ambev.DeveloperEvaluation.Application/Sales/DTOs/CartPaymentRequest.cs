namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class CartPaymentRequest
    {
        public string CardName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string CardExpiration { get; set; } = null!;
        public string CardCvv { get; set; } = null!;
    }
}
