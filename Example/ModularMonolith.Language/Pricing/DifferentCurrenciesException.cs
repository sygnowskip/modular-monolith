using System;

namespace ModularMonolith.Language.Pricing
{
    public class DifferentCurrenciesException : Exception
    {
        public string LeftCurrency { get; }

        public string RightCurrency { get; }

        public DifferentCurrenciesException(string leftCurrency, string rightCurrency)
            : base("Cannot operate over objects with different currencies")
        {
            LeftCurrency = leftCurrency;
            RightCurrency = rightCurrency;
        }
    }
}