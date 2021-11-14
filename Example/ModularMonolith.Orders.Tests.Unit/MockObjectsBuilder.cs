using System;
using Hexure.Results;
using Hexure.Time;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;
using Moq;

namespace ModularMonolith.Orders.Tests.Unit
{
    internal static class MockObjectsBuilder
    {
        public static IGrossPricePolicy BuildGrossPricePolicy(bool isSuccess)
        {
            var mock = new Mock<IGrossPricePolicy>();
            mock.Setup(policy =>
                    policy.IsGrossPriceSumOfNetAndTax(It.IsAny<Price>(), It.IsAny<Tax>(), It.IsAny<Price>()))
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