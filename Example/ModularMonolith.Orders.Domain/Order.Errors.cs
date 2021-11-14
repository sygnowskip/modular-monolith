using Hexure.Results;

namespace ModularMonolith.Orders.Domain
{
    public static class OrderErrors
    {
        public static class GreaterThanZero
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(GreaterThanZero), "Value has to be greater than zero");
            
            public static Result Check(decimal value, string property)
            {
                return Result.Create(0 < value, Error.Build().SetPropertyName(property));
            }
            
            public static Result Check(decimal value)
            {
                return Result.Create(0 < value, Error.Build());
            }
        }
        
        public static class GreaterThanOrEqualZero
        {
            public static readonly Error.ErrorType Error =
                new Error.ErrorType(nameof(GreaterThanOrEqualZero), "Value has to be greater than or equal zero");
            
            public static Result Check(decimal value, string property)
            {
                return Result.Create(0 <= value, Error.Build().SetPropertyName(property));
            }
            
            public static Result Check(decimal value)
            {
                return Result.Create(0 <= value, Error.Build());
            }
        }
    }
}