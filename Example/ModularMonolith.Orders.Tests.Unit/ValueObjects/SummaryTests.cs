using System.Collections.Generic;
using FluentAssertions;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.ValueObjects
{
    [TestFixture]
    public class SummaryTests
    {
        [TestCaseSource(nameof(SummaryTestCases))]
        public void ShouldReturnExpectedResult(IReadOnlyCollection<Item> items, IGrossPricePolicy grossPricePolicy, bool expected, string because)
        {
            var itemResult = Summary.Create(items, grossPricePolicy);

            itemResult.IsSuccess.Should().Be(expected, because);
        }

        private static IEnumerable<TestCaseData> SummaryTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new List<Item>()
                    {
                       TestObjectsBuilder.CreateItem("REG~123"), 
                       TestObjectsBuilder.CreateItem("REG~124") 
                    }.AsReadOnly(),
                    MockObjectsBuilder.BuildGrossPricePolicy(true), 
                    true, "It's correct summary");
                yield return new TestCaseData(
                    new List<Item>().AsReadOnly(),
                    MockObjectsBuilder.BuildGrossPricePolicy(true), 
                    false, "It's incorrect summary - items collection is empty");
                yield return new TestCaseData(
                    new List<Item>()
                    {
                       TestObjectsBuilder.CreateItem("REG~123"), 
                       TestObjectsBuilder.CreateItem("REG~124") 
                    }.AsReadOnly(),
                    MockObjectsBuilder.BuildGrossPricePolicy(false), 
                    false, "It's incorrect summary - gross is not a sum of net and tax");
            }
        }
    }
}