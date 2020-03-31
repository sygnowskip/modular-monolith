using System;
using FluentAssertions;
using ModularMonolith.Registrations.Language.ValueObjects;
using ModularMonolith.Time;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Registrations.ValueObjects
{
    [TestFixture]
    public class DateOfBirthTests
    {
        private readonly Mock<ISystemTimeProvider> _systemTimeProviderMock = new Mock<ISystemTimeProvider>();

        public DateOfBirthTests()
        {
            _systemTimeProviderMock.Setup(provider => provider.UtcNow)
                .Returns(() => new DateTime(2020, 03, 01));
        }

        [Test]
        public void ShouldCreateDateOfBirthForPastDates()
        {
            var dateOfBirth = DateOfBirth.Create(new DateTime(1980, 03, 01), _systemTimeProviderMock.Object);

            dateOfBirth.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ShouldNotCreateDateOfBirthForFutureDates()
        {
            var dateOfBirth = DateOfBirth.Create(new DateTime(2030, 03, 01), _systemTimeProviderMock.Object);

            dateOfBirth.IsSuccess.Should().BeFalse();
        }
    }
}