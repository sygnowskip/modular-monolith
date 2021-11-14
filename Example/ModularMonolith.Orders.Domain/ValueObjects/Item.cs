using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Orders.Domain.Policies;

namespace ModularMonolith.Orders.Domain.ValueObjects
{
    public class Item : ValueObject
    {
        protected Item(string name, string externalId, Quantity quantity, Price netPrice, Tax tax, Price grossPrice)
        {
            Name = name;
            ExternalId = externalId;
            Quantity = quantity;
            NetPrice = netPrice;
            Tax = tax;
            GrossPrice = grossPrice;
        }

        public string Name { get; }
        public string ExternalId { get; }
        public Quantity Quantity { get; }
        public Price NetPrice { get; }
        public Tax Tax { get; }
        public Price GrossPrice { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return ExternalId;
            yield return Quantity;
            yield return NetPrice;
            yield return Tax;
            yield return GrossPrice;
        }

        public static Result<Item> Create(string name, string externalId, int quantity, decimal netPrice, decimal tax,
            decimal grossPrice, IGrossPricePolicy grossPricePolicy)
        {
            var quantityResult = Quantity.Create(quantity);
            var netPriceResult = Price.Create(netPrice);
            var taxResult = Tax.Create(tax);
            var grossPriceResult = Price.Create(grossPrice);
            
            return Result.Combine(
                    quantityResult, netPriceResult, taxResult, grossPriceResult,
                    ModularMonolith.Language.Errors.NotNullOrWhiteSpace.Check(name, nameof(Name)),
                    ModularMonolith.Language.Errors.NotNullOrWhiteSpace.Check(externalId, nameof(ExternalId)))
                .OnSuccess(() => grossPricePolicy.IsGrossPriceSumOfNetAndTax(netPriceResult.Value, taxResult.Value, grossPriceResult.Value))
                .OnSuccess(() => new Item(name, externalId, quantityResult.Value, netPriceResult.Value, taxResult.Value, grossPriceResult.Value));
        }
    }
}