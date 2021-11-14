using FluentAssertions;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.ValueObjects
{
    [TestFixture]
    public class PriceTests
    {
        [TestCase(10, true, "It's correct value for price")]
        [TestCase(9.99, true, "It's correct value for price")]
        [TestCase(-10, false, "It's incorrect value for price")]
        [TestCase(0, false, "It's incorrect value for price")]
        public void ShouldReturnExpectedResult(decimal value, bool expected, string because)
        {
            var priceResult = Price.Create(value);

            priceResult.IsSuccess.Should().Be(expected, because);
        }
    }
}