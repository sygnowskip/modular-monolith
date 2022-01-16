using System;
using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Language.Pricing;

namespace ModularMonolith.Orders.Domain.ValueObjects
{
    public class OrderItem : ValueObject
    {
        //EF constructor
        protected OrderItem() {}
        
        protected OrderItem(string name, string productType, Guid externalId, Quantity quantity, Price price)
        {
            Name = name;
            ProductType = productType;
            ExternalId = externalId;
            Quantity = quantity;
            Price = price;
        }

        public string Name { get; }
        public Guid ExternalId { get; }
        public string ProductType { get; }
        public Quantity Quantity { get; }
        public Price Price { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return ExternalId;
            yield return Quantity;
            yield return Price;
            yield return ProductType;
        }

        public static Result<OrderItem> Create(string name, string productType, Guid externalId, int quantity, Price price)
        {
            var quantityResult = Quantity.Create(quantity);

            return Result.Combine(
                    quantityResult,
                    ModularMonolith.Language.CommonErrors.NotNullOrWhiteSpace.Check(name, nameof(Name)),
                    ModularMonolith.Language.CommonErrors.NotEmpty.Check(externalId, nameof(ExternalId)))
                .OnSuccess(() => new OrderItem(name, productType, externalId, quantityResult.Value, price));
        }
    }
}