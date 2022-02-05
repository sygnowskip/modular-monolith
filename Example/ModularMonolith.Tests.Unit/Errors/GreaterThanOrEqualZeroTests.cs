using FluentAssertions;
using ModularMonolith.Language;
using ModularMonolith.Language.Pricing;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Errors
{
    public class GreaterThanOrEqualZeroTests
    {
        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(1, true)]
        public void ShouldReturnExpectedResult(decimal value, bool expected)
        {
            var money = Money.Create(value, SupportedCurrencies.CHF()).Value;
            
            var result = CommonErrors.GreaterThanOrEqualZero.Check(money);

            result.IsSuccess.Should().Be(expected);
        }
        
        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(1, true)]
        public void ShouldReturnExpectedResult(int value, bool expected)
        {
            var result = CommonErrors.GreaterThanOrEqualZero.Check(value);

            result.IsSuccess.Should().Be(expected);
        }
    }
}