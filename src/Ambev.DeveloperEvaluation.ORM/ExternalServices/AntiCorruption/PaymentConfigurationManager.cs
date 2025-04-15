using Ambev.DeveloperEvaluation.ORM.ExternalServices.AntiCorruption.Interfaces;

namespace Ambev.DeveloperEvaluation.ORM.ExternalServices.AntiCorruption
{
    public class PagamentoConfigurationManager : IPaymentConfigurationManager
    {
        public string GetValue(string node)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
