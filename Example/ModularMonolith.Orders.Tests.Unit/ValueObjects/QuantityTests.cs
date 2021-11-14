using FluentAssertions;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.ValueObjects
{
    [TestFixture]
    public class QuantityTests
    {
        [TestCase(10, true, "It's correct value for quantity")]
        [TestCase(0, false, "It's incorrect value for quantity")]
        [TestCase(-10, false, "It's incorrect value for quantity")]
        public void ShouldReturnExpectedResult(int value, bool expected, string because)
        {
            var quantityResult = Quantity.Create(value);

            quantityResult.IsSuccess.Should().Be(expected, because);
        }
    }
}