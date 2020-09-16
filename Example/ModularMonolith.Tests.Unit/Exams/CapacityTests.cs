using FluentAssertions;
using ModularMonolith.Exams.Domain.ValueObjects;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Exams
{
    [TestFixture]
    public class CapacityTests
    {
        [TestCase(-10, false)]
        [TestCase(0, false)]
        [TestCase(10, true)]
        public void ShouldReturnExpectedResult(int capacity, bool isSuccess)
        {
            var capacityResult = Capacity.Create(capacity);

            capacityResult.IsSuccess.Should().Be(isSuccess);

            if (isSuccess)
                capacityResult.Value.Value.Should().Be(capacity);
        }
    }
}