using ModularMonolith.Orders.Domain.ValueObjects;

namespace ModularMonolith.Orders.Domain
{
    public static class Company
    {
        public static ContactData DefaultSeller =
            ContactData.Create("Company Inc.", "Business Street 12/344", "Warsaw", "00-999").Value;
    }
}