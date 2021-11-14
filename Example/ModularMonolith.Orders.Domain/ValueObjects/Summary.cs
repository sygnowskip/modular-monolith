using System.Collections.Generic;
using System.Linq;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Orders.Domain.Policies;

namespace ModularMonolith.Orders.Domain.ValueObjects
{
    public class Summary : ValueObject
    {
        protected Summary(Price netPrice, Tax tax, Price grossPrice)
        {
            NetPrice = netPrice;
            Tax = tax;
            GrossPrice = grossPrice;
        }

        public Price NetPrice { get; }
        public Tax Tax { get; }
        public Price GrossPrice { get; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NetPrice;
            yield return Tax;
            yield return GrossPrice;
        }

        public static Result<Summary> Create(IReadOnlyCollection<Item> items, IGrossPricePolicy grossPricePolicy)
        {
            var netPriceResult = Price.Create(items.Sum(i => i.NetPrice.Value));
            var taxResult = ValueObjects.Tax.Create(items.Sum(i => i.Tax.Value));
            var grossPriceResult = Price.Create(items.Sum(i => i.GrossPrice.Value));

            return Result.Create(items.Any(), Errors.CannotCreateSummaryForEmptyListItems.Build())
                .OnSuccess(() => Result.Combine(netPriceResult, taxResult, grossPriceResult))
                .OnSuccess(() => grossPricePolicy.IsGrossPriceSumOfNetAndTax(netPriceResult.Value, taxResult.Value, grossPriceResult.Value))
                .OnSuccess(() => new Summary(netPriceResult.Value, taxResult.Value, grossPriceResult.Value));
        }

        public static class Errors
        {
            public static readonly Error.ErrorType CannotCreateSummaryForEmptyListItems =
                new Error.ErrorType(nameof(CannotCreateSummaryForEmptyListItems),
                    "Cannot create summary for empty collection of items");
        }
    }
}