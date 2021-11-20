using Hexure.Results;

namespace ModularMonolith.Language.Pricing
{
    public interface ISingleCurrencyPolicy
    {
        Result IsSingleCurrency(Money net, Money tax);
    }
    
    public class SingleCurrencyPolicy : ISingleCurrencyPolicy
    {
        public Result IsSingleCurrency(Money net, Money tax)
        {
            return Result.Create(net.Currency == tax.Currency, Errors.NetAndTaxValueShouldHaveTheSameCurrency.Build());
        }

        public static void CheckSingleCurrencyOrThrow(Money left, Money right)
        {
            if (left.Currency == null || right.Currency == null || left.Currency != right.Currency)
                throw new DifferentCurrenciesException(left.Currency?.ShortCode, right.Currency?.ShortCode);
        }
        
        public static class Errors
        {
            public static readonly Error.ErrorType NetAndTaxValueShouldHaveTheSameCurrency =
                new Error.ErrorType(nameof(NetAndTaxValueShouldHaveTheSameCurrency),
                    "Net value and tax value should have the same currency");
        }
    }
}