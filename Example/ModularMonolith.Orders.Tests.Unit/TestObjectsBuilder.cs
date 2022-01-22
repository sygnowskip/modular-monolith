using System;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Orders.Domain.ValueObjects;

namespace ModularMonolith.Orders.Tests.Unit
{
    internal static class TestObjectsBuilder
    {
        public static Price CreatePrice(decimal net, decimal tax)
        {
            var netResult = Money.Create(net, SupportedCurrencies.USD());
            var taxResult = Money.Create(tax, SupportedCurrencies.USD());
            return Price.Create(netResult.Value, taxResult.Value, MockObjectsBuilder.BuildSingleCurrencyPolicy(true)).Value;
        }
        
        public static Price CreatePrice(decimal net, decimal tax, Currency currency)
        {
            var netResult = Money.Create(net, currency);
            var taxResult = Money.Create(tax, currency);
            return Price.Create(netResult.Value, taxResult.Value, MockObjectsBuilder.BuildSingleCurrencyPolicy(true)).Value;
        }

        public static OrderItem CreateItem(Guid externalId)
        {
            return OrderItem.Create("Item", "PRODUCT", externalId, 1, CreatePrice(100, 23)).Value;
        }

        public static OrderItem CreateItem(Guid externalId, Currency currency)
        {
            return OrderItem.Create("Item", "PRODUCT", externalId, 1, CreatePrice(100, 23, currency)).Value;
        }

        public static ContactData CreateContactData()
        {
            return ContactData.Create("John Smith", "Common 13B", "New York", "12-345").Value;
        }
    }
}