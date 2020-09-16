using System;
using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Exams.Domain.ValueObjects
{
    public class UtcDateTime : ValueObject
    {
        private UtcDateTime(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<UtcDateTime> Create(DateTime utcDateTime)
        {
            return Result.Create(() => utcDateTime.Kind == DateTimeKind.Utc, UtcDateErrors.DateHasToBeInUtc.Build())
                .OnSuccess(() => new UtcDateTime(utcDateTime));
        }
    }
}