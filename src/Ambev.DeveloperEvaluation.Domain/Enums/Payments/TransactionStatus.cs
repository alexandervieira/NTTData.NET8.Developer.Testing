namespace Ambev.DeveloperEvaluation.Domain.Enums.Payments
{
    public enum TransactionStatus
    {
        Paid = 1,
        Pending = 2,
        Canceled = 3,
        Refunded = 4,
        Failed = 5,
        Chargeback = 6,
        Declined = 7
    }
}
