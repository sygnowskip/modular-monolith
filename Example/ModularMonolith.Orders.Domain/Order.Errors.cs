using Hexure.Results;

namespace ModularMonolith.Orders.Domain
{
    public static class OrderErrors
    {
        public static readonly Error.ErrorType CannotCreateOrderForEmptyListItems =
            new Error.ErrorType(nameof(CannotCreateOrderForEmptyListItems),
                "Cannot create order for empty collection of items");
    }
}