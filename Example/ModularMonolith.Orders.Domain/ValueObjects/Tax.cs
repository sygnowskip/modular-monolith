using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Orders.Domain.ValueObjects
{
    public class Tax : ValueObject
    {
        protected Tax(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Tax> Create(decimal value)
        {
            return OrderErrors.GreaterThanOrEqualZero.Check(value)
                .OnSuccess(() => new Tax(value));
        }
    }
}