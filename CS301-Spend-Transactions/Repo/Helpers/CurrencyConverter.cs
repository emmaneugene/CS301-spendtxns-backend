using System.Collections.Generic;

namespace CS301_Spend_Transactions.Repo.Helpers
{
    /**
     * Class that converts foreign currencies to SGD - hardcoded values for convenience
     */
    public class CurrencyConverter
    {
        private static readonly Dictionary<string, decimal> ConversionChart = new Dictionary<string, decimal>()
        {
            {"USD", 1.35m},
            {"EUR", 1.55m},
            {"GBP", 1.82m},
            {"INR", 0.0182m},
            {"AUD", 1.00m},
            {"CAD", 1.08m},
            {"JPY", 0.119m},
            {"MYR", 0.325m},
            {"CNY", 0.211m},
        };
        
        public static decimal ConvertToSgd(string currency, decimal amount)
        {
            return amount * ConversionChart[currency];
        }
    }
}