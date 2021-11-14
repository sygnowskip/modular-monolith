using System;
using System.Collections.Generic;
using FluentAssertions;
using Hexure.Time;
using ModularMonolith.Orders.Domain;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit
{
    [TestFixture]
    public class OrderCreationTests
    {
        [TestCaseSource(nameof(OrderTestCases))]
        public void ShouldReturnExpectedResult(ContactData seller, ContactData buyer, IReadOnlyCollection<Item> items,
            ISystemTimeProvider systemTimeProvider, IGrossPricePolicy grossPricePolicy, bool expected, string because)
        {
            var orderResult = Order.Create(seller, buyer, items, systemTimeProvider, grossPricePolicy);

            orderResult.IsSuccess.Should().Be(expected, because);
        }
        
        private static IEnumerable<TestCaseData> OrderTestCases
        {
            get
            {
                yield return new TestCaseData(
                    TestObjectsBuilder.CreateContactData(),
                    TestObjectsBuilder.CreateContactData(),
                    new List<Item>()
                    {
                        TestObjectsBuilder.CreateItem("REG~123"), 
                        TestObjectsBuilder.CreateItem("REG~124") 
                    }.AsReadOnly(),
                    MockObjectsBuilder.BuildSystemTimeProvider(DateTime.UtcNow),
                    MockObjectsBuilder.BuildGrossPricePolicy(true),
                    true, "It's correct order");
            }
        }
    }
}