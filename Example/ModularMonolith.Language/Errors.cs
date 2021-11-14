using Hexure.Results;

namespace ModularMonolith.Language
{
    public static class Errors
    {
        public static class NotNullOrWhiteSpace
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(NotNullOrWhiteSpace), "Field cannot be empty ({0})");
            
            public static Result Check(string value, string property)
            {
                return Result.Create(!string.IsNullOrWhiteSpace(value), Error.Build(property).SetPropertyName(property));
            }
        }
        
        public static class MaxLength
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(MaxLength), "Field cannot be longer than {0} characters ({1})");
            
            public static Result Check(string value, int maxLength, string property)
            {
                var valueLength = value?.Length ?? 0;
                return Result.Create(valueLength <= maxLength, Error.Build(maxLength, property).SetPropertyName(property));
            }
        }
    }
}