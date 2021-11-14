using FluentAssertions;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.ValueObjects
{
    [TestFixture]
    public class TaxTests
    {
        [TestCase(10, true, "It's correct value for tax")]
        [TestCase(9.99, true, "It's correct value for tax")]
        [TestCase(0, true, "It's correct value for tax")]
        [TestCase(-10, false, "It's incorrect value for tax")]
        public void ShouldReturnExpectedResult(decimal value, bool expected, string because)
        {
            var taxResult = Tax.Create(value);

            taxResult.IsSuccess.Should().Be(expected, because);
        }
    }
}