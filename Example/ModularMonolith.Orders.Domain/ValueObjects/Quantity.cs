using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Language;

namespace ModularMonolith.Orders.Domain.ValueObjects
{
    public class Quantity : ValueObject
    {
        protected Quantity(int value)
        {
            Value = value;
        }

        public decimal Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Quantity> Create(int value)
        {
            return CommonErrors.GreaterThanZero.Check(value)
                .OnSuccess(() => new Quantity(value));
        }
    }
}