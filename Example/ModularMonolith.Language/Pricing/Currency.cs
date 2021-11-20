using System.Collections.Generic;
using Hexure.Results;

namespace ModularMonolith.Language.Pricing
{
    public class Currency : ValueObject
    {
        internal Currency(string shortCode)
        {
            ShortCode = shortCode;
        }

        public string ShortCode { get; private set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ShortCode;
        }
    }
    
    public static class SupportedCurrencies
    {
        public static Currency USD = new Currency(nameof(USD));
        public static Currency EUR = new Currency(nameof(EUR));
        public static Currency CHF = new Currency(nameof(CHF));
        public static Currency GBP = new Currency(nameof(GBP));
    }
}