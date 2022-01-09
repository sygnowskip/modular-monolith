using System;
using Hexure.Results;
using ModularMonolith.Language.Pricing;

namespace ModularMonolith.Language
{
    public static class CommonErrors
    {
        public static class NotNullOrWhiteSpace
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(NotNullOrWhiteSpace), "Field cannot be empty ({0})");

            public static Result Check(string value, string property)
            {
                return Result.Create(!string.IsNullOrWhiteSpace(value),
                    Error.Build(property).SetPropertyName(property));
            }
        }
        
        public static class NotEmpty
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(NotNullOrWhiteSpace), "Field cannot be empty ({0})");

            public static Result Check(Guid value, string property)
            {
                return Result.Create(value != Guid.Empty,
                    Error.Build(property).SetPropertyName(property));
            }
        }

        public static class MaxLength
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(MaxLength), "Field cannot be longer than {0} characters ({1})");

            public static Result Check(string value, int maxLength, string property)
            {
                var valueLength = value?.Length ?? 0;
                return Result.Create(valueLength <= maxLength,
                    Error.Build(maxLength, property).SetPropertyName(property));
            }
        }

        public static class GreaterThanZero
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(GreaterThanZero), "Value has to be greater than zero");

            public static Result Check(Money value)
            {
                return Result.Create(0 < value.Amount, Error.Build());
            }
            
            public static Result Check(int value)
            {
                return Result.Create(0 < value, Error.Build());
            }
        }

        public static class GreaterThanOrEqualZero
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(GreaterThanOrEqualZero), "Value has to be greater than or equal zero");

            public static Result Check(Money value)
            {
                return Result.Create(0 <= value.Amount, Error.Build());
            }
            
            public static Result Check(int value)
            {
                return Result.Create(0 <= value, Error.Build());
            }
        }
    }
}