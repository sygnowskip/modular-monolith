using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using ModularMonolith.Language;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;

namespace ModularMonolith.Orders.Domain
{
    public partial class Order
    {
        public static Result<Order> Create(ContactData seller, ContactData buyer, IReadOnlyCollection<Item> items, 
            ISystemTimeProvider systemTimeProvider, IGrossPricePolicy grossPricePolicy)
        {
            var creationDateResult = UtcDate.Create(systemTimeProvider.UtcNow.Date);
            var summaryResult = Summary.Create(items, grossPricePolicy);

            return Result.Combine(creationDateResult, summaryResult)
                .OnSuccess(() => new Order(creationDateResult.Value, seller, buyer, items, summaryResult.Value,
                    systemTimeProvider));
        }
    }
}