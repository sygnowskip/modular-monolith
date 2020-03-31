using System;
using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Time;

namespace ModularMonolith.Registrations.Language.ValueObjects
{
    public static class DateOfBirthErrors
    {
        public static Error.ErrorType ShouldBeInThePast = new Error.ErrorType(nameof(ShouldBeInThePast), "Date of birth should be in the past");
    }

    public class DateOfBirth : ValueObject
    {
        private DateOfBirth(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<DateOfBirth> Create(DateTime dateOfBirth, ISystemTimeProvider systemTimeProvider)
        {
            return Result.Create(dateOfBirth < systemTimeProvider.UtcNow, DateOfBirthErrors.ShouldBeInThePast.Build())
                .OnSuccess(() => new DateOfBirth(dateOfBirth));
        }
    }
}