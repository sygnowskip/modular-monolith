using System;
using System.Collections.Generic;
using Hexure.Results;
using Hexure.Time;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;
using Moq;

namespace ModularMonolith.Orders.Tests.Unit
{
    internal static class MockObjectsBuilder
    {
        public static ISingleItemsCurrencyPolicy BuildSingleItemsCurrencyPolicy(bool isSuccess)
        {
            var mock = new Mock<ISingleItemsCurrencyPolicy>();
            mock.Setup(policy => policy.AllItemsHaveSingleCurrency(It.IsAny<IReadOnlyCollection<Item>>()))
                .Returns(() => isSuccess ? Result.Ok() : Result.Fail(Errors.CommonErrorType.Build()));
            return mock.Object;
        }
        
        public static ISingleCurrencyPolicy BuildSingleCurrencyPolicy(bool isSuccess)
        {
            var mock = new Mock<ISingleCurrencyPolicy>();
            mock.Setup(policy =>
                    policy.IsSingleCurrency(It.IsAny<Money>(), It.IsAny<Money>()))
                .Returns(() => isSuccess ? Result.Ok() : Result.Fail(Errors.CommonErrorType.Build()));
            return mock.Object;
        }
        
        public static ISystemTimeProvider BuildSystemTimeProvider(DateTime utcNow)
        {
            var mock = new Mock<ISystemTimeProvider>();
            mock.Setup(provider => provider.UtcNow)
                .Returns(utcNow);
            return mock.Object;
        }

        private static class Errors
        {
            public static Error.ErrorType CommonErrorType =
                new Error.ErrorType(nameof(CommonErrorType), "Mock error type");
        }
    }
}