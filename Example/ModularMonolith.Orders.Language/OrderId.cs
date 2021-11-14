using Hexure.Identifiers.Numeric;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Errors;
using Newtonsoft.Json;

namespace ModularMonolith.Orders.Language
{
    public sealed class OrderId : Identifier
    {
        [JsonConstructor]
        internal OrderId(long value) : base(value)
        {
        }
        
        public static Result<OrderId> Create(long orderId)
        {
            return Result.Create(() => orderId > 0, DomainErrors.BuildInvalidIdentifier(orderId))
                .OnSuccess(() => new OrderId(orderId));
        }
    }
}