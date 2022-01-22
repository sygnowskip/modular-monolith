using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public static Result<Order> Create(ContactData seller, ContactData buyer, IReadOnlyCollection<OrderItem> items,
            ISystemTimeProvider systemTimeProvider, ISingleCurrencyPolicy singleCurrencyPolicy, ISingleItemsCurrencyPolicy singleItemsCurrencyPolicy)
        {
            var creationDateResult = UtcDateTime.Create(systemTimeProvider.UtcNow);
            var summaryResult = CreateSummary(items, singleItemsCurrencyPolicy, singleCurrencyPolicy);

            return Result.Combine(creationDateResult, summaryResult)
                .OnSuccess(() => new Order(creationDateResult.Value, seller, buyer, items, summaryResult.Value,
                    systemTimeProvider));
        }
        
        public static Task<Result<Order>> CreateWithDefaultSellerAsync(ContactData buyer, IReadOnlyCollection<OrderItem> items,
            ISystemTimeProvider systemTimeProvider, ISingleCurrencyPolicy singleCurrencyPolicy, ISingleItemsCurrencyPolicy singleItemsCurrencyPolicy,
            IOrderRepository orderRepository)
        {
            return Create(Company.DefaultSeller, buyer, items, systemTimeProvider, singleCurrencyPolicy,
                singleItemsCurrencyPolicy)
                .OnSuccess(async order => await orderRepository.SaveAsync(order));
        }

        private static Result<Price> CreateSummary(IReadOnlyCollection<OrderItem> items,
            ISingleItemsCurrencyPolicy singleItemsCurrencyPolicy, ISingleCurrencyPolicy singleCurrencyPolicy)
        {
            return Result.Create(items.Any(), OrderErrors.CannotCreateOrderForEmptyListItems.Build())
                .OnSuccess(() => singleItemsCurrencyPolicy.AllItemsHaveSingleCurrency(items))
                .OnSuccess(() =>
                {
                    var currency = items.Select(i => i.Price.Net.Currency).Distinct().Single();
                    var netResult = Money.Create(items.Sum(i => i.Price.Net.Amount), currency.Clone());
                    var taxResult = Money.Create(items.Sum(i => i.Price.Tax.Amount), currency.Clone());

                    return Result.Combine(netResult, taxResult)
                        .OnSuccess(() => Price.Create(netResult.Value, taxResult.Value, singleCurrencyPolicy));
                });
        }
    }
}