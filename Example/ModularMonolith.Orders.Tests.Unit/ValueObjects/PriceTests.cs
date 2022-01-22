using FluentAssertions;
using ModularMonolith.Language.Pricing;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.ValueObjects
{
    [TestFixture]
    public class PriceTests
    {
        [TestCase(10, 0, true, "It's correct value for price")]
        [TestCase(10, 2.3,true, "It's correct value for price")]
        [TestCase(-10, 2.3, false, "It's incorrect value for price (net <= 0)")]
        [TestCase(0, 2.3, false, "It's incorrect value for price (net <= 0)")]
        [TestCase(10, -10, false, "It's incorrect value for price  (tax < 0)")]
        public void ShouldReturnExpectedResult(decimal net, decimal tax, bool expected, string because)
        {
            var netValue = Money.Create(net, SupportedCurrencies.USD()).Value;
            var taxValue = Money.Create(tax, SupportedCurrencies.USD()).Value;
            
            var priceResult = Price.Create(netValue, taxValue, MockObjectsBuilder.BuildSingleCurrencyPolicy(true));

            priceResult.IsSuccess.Should().Be(expected, because);
        }
    }
}