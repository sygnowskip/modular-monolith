using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Exams.Domain.ValueObjects
{
    public class Booked : ValueObject
    {
        private Booked(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public Result<Booked> Increase() => Create(Value + 1);
        public Result<Booked> Decrease() => Create(Value - 1);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Booked> Create(int booked)
        {
            return Result.Create(() => booked >= 0, CapacityErrors.CapacityHasToBeGreaterThanZero.Build())
                .OnSuccess(() => new Booked(booked));
        }

        public static Booked Zero = new Booked(0);
    }

    public static class BookedErrors
    {
        public static readonly Error.ErrorType BookedHasToBeGreaterOrEqualZero =
            new Error.ErrorType(nameof(BookedHasToBeGreaterOrEqualZero), "Booked has to be greater or equal zero");
    }
}