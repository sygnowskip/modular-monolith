using System;
using System.Collections.Generic;
using FluentAssertions;
using Hexure.Time;
using ModularMonolith.Language.Pricing;
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
        public void ShouldReturnExpectedResult(ContactData seller, ContactData buyer, IReadOnlyCollection<OrderItem> items,
            ISystemTimeProvider systemTimeProvider, ISingleCurrencyPolicy singleCurrencyPolicy, ISingleItemsCurrencyPolicy singleItemsCurrencyPolicy, bool expected, string because)
        {
            var orderResult = Order.Create(seller, buyer, items, systemTimeProvider, singleCurrencyPolicy, singleItemsCurrencyPolicy);

            orderResult.IsSuccess.Should().Be(expected, because);
        }
        
        private static IEnumerable<TestCaseData> OrderTestCases
        {
            get
            {
                yield return new TestCaseData(
                    TestObjectsBuilder.CreateContactData(),
                    TestObjectsBuilder.CreateContactData(),
                    new List<OrderItem>()
                    {
                        TestObjectsBuilder.CreateItem(Guid.NewGuid()), 
                        TestObjectsBuilder.CreateItem(Guid.NewGuid()) 
                    }.AsReadOnly(),
                    MockObjectsBuilder.BuildSystemTimeProvider(DateTime.UtcNow),
                    MockObjectsBuilder.BuildSingleCurrencyPolicy(true),
                    MockObjectsBuilder.BuildSingleItemsCurrencyPolicy(true),
                    true, "It's correct order");
                yield return new TestCaseData(
                    TestObjectsBuilder.CreateContactData(),
                    TestObjectsBuilder.CreateContactData(),
                    new List<OrderItem>()
                    {
                        TestObjectsBuilder.CreateItem(Guid.NewGuid()), 
                        TestObjectsBuilder.CreateItem(Guid.NewGuid()) 
                    }.AsReadOnly(),
                    MockObjectsBuilder.BuildSystemTimeProvider(DateTime.UtcNow),
                    MockObjectsBuilder.BuildSingleCurrencyPolicy(true),
                    MockObjectsBuilder.BuildSingleItemsCurrencyPolicy(false),
                    false, "Order contains items with different currencies");
                yield return new TestCaseData(
                    TestObjectsBuilder.CreateContactData(),
                    TestObjectsBuilder.CreateContactData(),
                    new List<OrderItem>().AsReadOnly(),
                    MockObjectsBuilder.BuildSystemTimeProvider(DateTime.UtcNow),
                    MockObjectsBuilder.BuildSingleCurrencyPolicy(true),
                    MockObjectsBuilder.BuildSingleItemsCurrencyPolicy(true),
                    false, "Order does not contains any items");
            }
        }
    }
}