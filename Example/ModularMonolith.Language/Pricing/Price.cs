using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Language.Pricing
{
    public class Price : ValueObject
    {
        //EF constructor
        protected Price(){ }
        
        private Price(Money net, Money tax)
        {
            Net = net;
            Tax = tax;
        }

        public Money Net { get; }
        public Money Tax { get; }
        public Money Gross => Net + Tax;
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
                .OnSuccess(() => new Price(net, tax));
        }
    }
}