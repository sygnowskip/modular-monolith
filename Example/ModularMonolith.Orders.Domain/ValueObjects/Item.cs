using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Language.Pricing;

namespace ModularMonolith.Orders.Domain.ValueObjects
{
    public class Item : ValueObject
    {
        protected Item(string name, string externalId, Quantity quantity, Price price)
        {
            Name = name;
            ExternalId = externalId;
            Quantity = quantity;
            Price = price;
        }

        public string Name { get; }
        public string ExternalId { get; }
        public Quantity Quantity { get; }
        public Price Price { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return ExternalId;
            yield return Quantity;
            yield return Price;
        }

        public static Result<Item> Create(string name, string externalId, int quantity, Price price)
        {
            var quantityResult = Quantity.Create(quantity);

            return Result.Combine(
                    quantityResult,
                    ModularMonolith.Language.CommonErrors.NotNullOrWhiteSpace.Check(name, nameof(Name)),
                    ModularMonolith.Language.CommonErrors.NotNullOrWhiteSpace.Check(externalId, nameof(ExternalId)))
                .OnSuccess(() => new Item(name, externalId, quantityResult.Value, price));
        }
    }
}