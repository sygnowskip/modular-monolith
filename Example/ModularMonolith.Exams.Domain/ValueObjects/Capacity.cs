using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Exams.Domain.ValueObjects
{
    public class Capacity : ValueObject
    {
        private Capacity(int value)
        {
            Value = value;
        }

        public int Value { get; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Capacity> Create(int capacity)
        {
            return Result.Create(() => capacity > 0, CapacityErrors.CapacityHasToBeGreaterThanZero.Build())
                .OnSuccess(() => new Capacity(capacity));
        }
    }

    public static class CapacityErrors
    {
        public static readonly Error.ErrorType CapacityHasToBeGreaterThanZero = new Error.ErrorType(nameof(CapacityHasToBeGreaterThanZero), "Capacity has to be greater than zero");
    }
}