using System.Collections.Generic;
using FluentAssertions;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.ValueObjects
{
    [TestFixture]
    public class ContactDataTests
    {
        [TestCaseSource(nameof(ContactDataTestsCases))]
        public void ShouldReturnExpectedResults(string name, string streetAddress, string city, string zipCode, bool expected, string because)
        {
            var result = ContactData.Create(name, streetAddress, city, zipCode);

            result.IsSuccess.Should().Be(expected, because);
        }
        
        private static IEnumerable<TestCaseData> ContactDataTestsCases
        {
            get
            {
                yield return new TestCaseData("John Smith", "Common 13B", "New York", "12-345", true, "It's correct value");
                yield return new TestCaseData("", "Common 13B", "New York", "12-345", false, "Name is missing");
                yield return new TestCaseData("John Smith", "", "New York", "12-345", false, "Street is missing");
                yield return new TestCaseData("John Smith", "Common 13B", "", "12-345", false, "City is missing");
                yield return new TestCaseData("John Smith", "Common 13B", "New York", "", false, "Zip Code is missing");
            }
        }
    }
}