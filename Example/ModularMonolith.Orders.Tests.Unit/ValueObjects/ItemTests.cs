using System.Collections.Generic;
using FluentAssertions;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.ValueObjects
{
    [TestFixture]
    public class ItemTests
    {
        [TestCaseSource(nameof(ItemTestCases))]
        public void ShouldReturnExpectedResult(string name, string externalId, int quantity, decimal net, decimal tax,
            decimal gross, IGrossPricePolicy grossPricePolicy, bool expected, string because)
        {
            var itemResult = Item.Create(name, externalId, quantity, net, tax, gross, grossPricePolicy);

            itemResult.IsSuccess.Should().Be(expected, because);
        }

        private static IEnumerable<TestCaseData> ItemTestCases
        {
            get
            {
                yield return new TestCaseData("John Smith", "REGISTRATION~123", 
                    3, 300M, 0M, 300M, MockObjectsBuilder.BuildGrossPricePolicy(true), 
                    true, "It's correct item");
                yield return new TestCaseData("", "REGISTRATION~123", 
                    3, 300M, 0M, 300M, MockObjectsBuilder.BuildGrossPricePolicy(true), 
                    false, "It's incorrect item - name is required");
                yield return new TestCaseData("John Smith", "", 
                    3, 300M, 0M, 300M, MockObjectsBuilder.BuildGrossPricePolicy(true), 
                    false, "It's incorrect item - external identifier is required");
                yield return new TestCaseData("John Smith", "REGISTRATION~123", 
                    3, 300M, 0M, 300M, MockObjectsBuilder.BuildGrossPricePolicy(false), 
                    false, "It's incorrect item - gross price is different than sum of net and tax");
            }
        }
    }
}