﻿namespace Ambev.DeveloperEvaluation.ORM.ExternalServices.AntiCorruption.Interfaces
{
    public interface IPayPalGateway
    {
        string GetPayPalServiceKey(string apiKey, string encriptionKey);
        string GetCardHashKey(string serviceKey, string cartaoCredito);
        bool CommitTransaction(string cardHashKey, string orderId, decimal amount);
    }
}
