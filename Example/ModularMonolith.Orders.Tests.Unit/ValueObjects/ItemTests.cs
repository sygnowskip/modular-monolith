﻿using System.Collections.Generic;
using FluentAssertions;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Orders.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Orders.Tests.Unit.ValueObjects
{
    [TestFixture]
    public class ItemTests
    {
        [TestCaseSource(nameof(ItemTestCases))]
        public void ShouldReturnExpectedResult(string name, string externalId, int quantity, Price price, bool expected, string because)
        {
            var itemResult = Item.Create(name, externalId, quantity, price);

            itemResult.IsSuccess.Should().Be(expected, because);
        }

        private static IEnumerable<TestCaseData> ItemTestCases
        {
            get
            {
                yield return new TestCaseData("John Smith", "REGISTRATION~123",
                    3, TestObjectsBuilder.CreatePrice(300M, 0),
                    true, "It's correct item");
                yield return new TestCaseData("", "REGISTRATION~123",
                    3, TestObjectsBuilder.CreatePrice(300M, 0),
                    false, "It's incorrect item - name is required");
                yield return new TestCaseData("John Smith", "",
                    3, TestObjectsBuilder.CreatePrice(300M, 0),
                    false, "It's incorrect item - external identifier is required");
            }
        }
    }
}