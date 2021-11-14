using System.Linq;
using FluentAssertions;
using ModularMonolith.Orders.Domain;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.Errors
{
    public class GreaterThanOrEqualZeroTests
    {
        [TestCase(-1)]
        public void ShouldReturnErrorWithPropertyName(decimal value)
        {
            var property = "NetPrice";

            var result = OrderErrors.GreaterThanOrEqualZero.Check(value, property);

            result.IsSuccess.Should().BeFalse();
            result.Error.Any(e => e.Message.Contains(property)).Should().BeTrue();
            result.Error.Any(e => e.PropertyName == property).Should().BeTrue();
        }
        
        [TestCase(0)]
        [TestCase(1)]
        public void ShouldReturnSuccess(decimal value)
        {
            var property = "NetPrice";

            var result = OrderErrors.GreaterThanOrEqualZero.Check(value, property);

            result.IsSuccess.Should().BeTrue();
        }
    }
}