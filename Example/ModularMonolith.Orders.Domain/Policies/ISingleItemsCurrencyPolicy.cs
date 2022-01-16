using System.Collections.Generic;
using System.Linq;
using Hexure.Results;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Orders.Domain.ValueObjects;

namespace ModularMonolith.Orders.Domain.Policies
{
    public interface ISingleItemsCurrencyPolicy
    {
        Result AllItemsHaveSingleCurrency(IReadOnlyCollection<OrderItem> items);
    }
    
    internal class SingleItemsCurrencyPolicy : ISingleItemsCurrencyPolicy
    {
        public Result AllItemsHaveSingleCurrency(IReadOnlyCollection<OrderItem> items)
        {
            var currencies = items.Select(i => i.Price.Net.Currency).Distinct().ToList();

            if (currencies.Count() != 1)
                return Result.Fail<Currency>(Errors.ItemsWithinSingleOrderShouldHaveTheSameCurrency.Build());

            return Result.Ok();
        }

        public static class Errors
        {
            public static readonly Error.ErrorType ItemsWithinSingleOrderShouldHaveTheSameCurrency =
                new Error.ErrorType(nameof(ItemsWithinSingleOrderShouldHaveTheSameCurrency),
                    "Items within single order should have the same currency");
        }
    }
}