using System.Collections.Generic;
using FluentAssertions;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.Policies
{
    [TestFixture]
    public class GrossPricePolicyTests
    {
        private readonly IGrossPricePolicy _grossPricePolicy = new GrossPricePolicy();

        [TestCaseSource(nameof(GrossPricePolicyTestCases))]
        public void ShouldReturnExpectedResult(Price net, Tax tax, Price gross, bool expected, string because)
        {
            var result = _grossPricePolicy.IsGrossPriceSumOfNetAndTax(net, tax, gross);

            result.IsSuccess.Should().Be(expected, because);
        }
        
        private static IEnumerable<TestCaseData> GrossPricePolicyTestCases
        {
            get
            {
                yield return new TestCaseData(TestObjectsBuilder.CreatePrice(10), TestObjectsBuilder.CreateTax(2.3M), TestObjectsBuilder.CreatePrice(12.3M), true, "It's correct value");
                yield return new TestCaseData(TestObjectsBuilder.CreatePrice(10), TestObjectsBuilder.CreateTax(1), TestObjectsBuilder.CreatePrice(13), false, "Sum is not correct");
                yield return new TestCaseData(TestObjectsBuilder.CreatePrice(10), TestObjectsBuilder.CreateTax(3), TestObjectsBuilder.CreatePrice(15), false, "Sum is not correct");
            }
        }
    }
}