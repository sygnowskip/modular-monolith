using Hexure.Results;

namespace ModularMonolith.Errors
{
    public static class DomainErrors
    {
        public static readonly Error.ErrorType NotFound = new Error.ErrorType(nameof(NotFound), "{0} for provided identifier {1} does not exist");
        public static Error BuildNotFound(string entity, object identifier) => NotFound.Build(entity, identifier);
        
        public static readonly Error.ErrorType AggregateNotFound = new Error.ErrorType(nameof(AggregateNotFound), "{0} for provided identifier {1} does not exist");
        public static Error BuildAggregateNotFound(string entity, object identifier) => AggregateNotFound.Build(entity, identifier);
        
        
        public static readonly Error.ErrorType InvalidIdentifier = new Error.ErrorType(nameof(InvalidIdentifier), "Provided identifier {0} is invalid");
        public static Error BuildInvalidIdentifier(object identifier) => InvalidIdentifier.Build(identifier);
    }
}