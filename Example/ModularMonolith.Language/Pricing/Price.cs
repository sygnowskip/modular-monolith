using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Language.Pricing
{
    public class Price : ValueObject
    {
        private Price(Money net, Money tax, Money gross)
        {
            Net = net;
            Tax = tax;
            Gross = gross;
        }

        public Money Net { get; }
        public Money Tax { get; }
        public Money Gross { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Net;
            yield return Tax;
            yield return Gross;
        }

        public static Result<Price> Create(Money net, Money tax, ISingleCurrencyPolicy singleCurrencyPolicy)
        {
            return Result.Combine(
                    CommonErrors.GreaterThanZero.Check(net),
                    CommonErrors.GreaterThanOrEqualZero.Check(tax)
                )
                .OnSuccess(() => singleCurrencyPolicy.IsSingleCurrency(net, tax))
                .OnSuccess(() => new Price(net, tax, net + tax));
        }
    }
}