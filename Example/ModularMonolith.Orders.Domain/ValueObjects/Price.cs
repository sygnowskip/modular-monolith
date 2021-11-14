using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Orders.Domain.ValueObjects
{
    public class Price : ValueObject
    {
        protected Price(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Price> Create(decimal value)
        {
            return OrderErrors.GreaterThanZero.Check(value)
                .OnSuccess(() => new Price(value));
        }
    }
}