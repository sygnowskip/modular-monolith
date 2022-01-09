using System.Collections.Generic;
using System.Linq;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using ModularMonolith.Language;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;

namespace ModularMonolith.Orders.Domain
{
    public partial class Order
    {
        public static Result<Order> Create(ContactData seller, ContactData buyer, IReadOnlyCollection<Item> items,
            ISystemTimeProvider systemTimeProvider, ISingleCurrencyPolicy singleCurrencyPolicy, ISingleItemsCurrencyPolicy singleItemsCurrencyPolicy)
        {
            var creationDateResult = UtcDate.Create(systemTimeProvider.UtcNow.Date);
            var summaryResult = CreateSummary(items, singleItemsCurrencyPolicy, singleCurrencyPolicy);

            return Result.Combine(creationDateResult, summaryResult)
                .OnSuccess(() => new Order(creationDateResult.Value, seller, buyer, items, summaryResult.Value,
                    systemTimeProvider));
        }
        
        public static Result<Order> CreateWithDefaultSeller(ContactData buyer, IReadOnlyCollection<Item> items,
            ISystemTimeProvider systemTimeProvider, ISingleCurrencyPolicy singleCurrencyPolicy, ISingleItemsCurrencyPolicy singleItemsCurrencyPolicy)
        {
            return Create(Company.DefaultSeller, buyer, items, systemTimeProvider, singleCurrencyPolicy,
                singleItemsCurrencyPolicy);
        }

        private static Result<Price> CreateSummary(IReadOnlyCollection<Item> items,
            ISingleItemsCurrencyPolicy singleItemsCurrencyPolicy, ISingleCurrencyPolicy singleCurrencyPolicy)
        {
            return Result.Create(items.Any(), OrderErrors.CannotCreateOrderForEmptyListItems.Build())
                .OnSuccess(() => singleItemsCurrencyPolicy.AllItemsHaveSingleCurrency(items))
                .OnSuccess(() =>
                {
                    var currency = items.Select(i => i.Price.Net.Currency).Distinct().Single();
                    var netResult = Money.Create(items.Sum(i => i.Price.Net.Amount), currency);
                    var taxResult = Money.Create(items.Sum(i => i.Price.Tax.Amount), currency);

                    return Result.Combine(netResult, taxResult)
                        .OnSuccess(() => Price.Create(netResult.Value, taxResult.Value, singleCurrencyPolicy));
                });
        }
    }
}