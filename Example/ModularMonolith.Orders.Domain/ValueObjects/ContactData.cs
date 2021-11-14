using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Orders.Domain.ValueObjects
{
    public class ContactData : ValueObject
    {
        public string Name { get; }
        public string StreetAddress { get; }
        public string City { get; }
        public string ZipCode { get; }

        private ContactData(string name, string streetAddress, string city, string zipCode)
        {
            Name = name;
            StreetAddress = streetAddress;
            City = city;
            ZipCode = zipCode;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return StreetAddress;
            yield return City;
            yield return ZipCode;
        }

        public static Result<ContactData> Create(string name, string streetAddress, string city, string zipCode)
        {
            return Result.Combine(
                    ModularMonolith.Language.Errors.NotNullOrWhiteSpace.Check(name, nameof(Name)),
                    ModularMonolith.Language.Errors.NotNullOrWhiteSpace.Check(streetAddress, nameof(StreetAddress)),
                    ModularMonolith.Language.Errors.NotNullOrWhiteSpace.Check(city, nameof(City)),
                    ModularMonolith.Language.Errors.NotNullOrWhiteSpace.Check(zipCode, nameof(ZipCode)))
                .OnSuccess(() => new ContactData(name, streetAddress, city, zipCode));
        }
    }
}