using ModularMonolith.Orders.Domain.ValueObjects;

namespace ModularMonolith.Orders.Tests.Unit
{
    internal static class TestObjectsBuilder
    {
        public static Price CreatePrice(decimal value)
        {
            return Price.Create(value).Value;
        }
        
        public static Tax CreateTax(decimal value)
        {
            return Tax.Create(value).Value;
        }

        public static Item CreateItem(string externalId)
        {
            return Item.Create("Item", externalId, 1, 100, 23, 123, 
                    MockObjectsBuilder.BuildGrossPricePolicy(true)).Value;
        }

        public static ContactData CreateContactData()
        {
            return ContactData.Create("John Smith", "Common 13B", "New York", "12-345").Value;
        }
    }
}