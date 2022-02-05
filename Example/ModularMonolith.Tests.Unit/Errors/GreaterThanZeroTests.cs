using FluentAssertions;
using ModularMonolith.Language;
using ModularMonolith.Language.Pricing;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Errors
{
    [TestFixture]
    public class GreaterThanZeroTests
    {
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void ShouldReturnExpectedResult(decimal value, bool expected)
        {
            var money = Money.Create(value, SupportedCurrencies.USD()).Value;

            var result = CommonErrors.GreaterThanZero.Check(money);

            result.IsSuccess.Should().Be(expected);
        }
        
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void ShouldReturnExpectedResult(int value, bool expected)
        {
            var result = CommonErrors.GreaterThanZero.Check(value);

            result.IsSuccess.Should().Be(expected);
        }
    }
}