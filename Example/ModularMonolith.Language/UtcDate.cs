using System;
using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Language
{
    public class UtcDate : ValueObject
    {
        private UtcDate(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<UtcDate> Create(DateTime utcDate)
        {
            return Result.Create(() => utcDate.Kind == DateTimeKind.Utc, UtcDateErrors.DateHasToBeInUtc.Build())
                .AndEnsure(() => utcDate.Date == utcDate, UtcDateErrors.DateCannotContainTime.Build())
                .OnSuccess(() => new UtcDate(utcDate));
        }
    }
    
    public class UtcDateErrors
    {
        public static readonly Error.ErrorType DateHasToBeInUtc = new Error.ErrorType(nameof(DateHasToBeInUtc), "Date has to be in UTC");
        public static readonly Error.ErrorType DateCannotContainTime = new Error.ErrorType(nameof(DateCannotContainTime), "Date has to be set to midnight time");
    }
}