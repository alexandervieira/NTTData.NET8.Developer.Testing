namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs
{
    public class CartPaymentResponse
    {
        public string CardName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string CardExpiration { get; set; } = null!;
        public string CardCvv { get; set; } = null!;
    }
}
