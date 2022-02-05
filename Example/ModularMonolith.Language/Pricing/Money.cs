using System.Collections.Generic;
using Hexure.Results;

namespace ModularMonolith.Language.Pricing
{
    public class Money : ValueObject
    {
        //EF constructor
        protected Money(){ }
        private Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public static Money operator +(Money left, Money right)
        {
            SingleCurrencyPolicy.CheckSingleCurrencyOrThrow(left, right);
            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Result<Money> Create(decimal amount, Currency currency)
        {
            return Result.Ok(new Money(amount, currency));
        }
    }
}