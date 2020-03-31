using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Registrations;
using ModularMonolith.Registrations.Language.ValueObjects;
using ModularMonolith.Time;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Registrations
{
    [TestFixture]
    public class RegistrationTests
    {
        private readonly Mock<ISystemTimeProvider> _systemTimeProviderMock;

        public RegistrationTests()
        {
            _systemTimeProviderMock = new Mock<ISystemTimeProvider>();
            _systemTimeProviderMock
                .Setup(provider => provider.UtcNow)
                .Returns(new DateTime(2020, 04, 01));
        }

        [Test]
        public void ShouldCreateRegistration()
        {
            var registration = DateOfBirth.Create(new DateTime(1980, 01, 01), _systemTimeProviderMock.Object)
                .OnSuccess(dob => Candidate.Create("John", "Smith", dob))
                .OnSuccess(Registration.Create);

            registration.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ShouldAddEventOnRegistrationCreation()
        {
            var registration = DateOfBirth.Create(new DateTime(1980, 01, 01), _systemTimeProviderMock.Object)
                .OnSuccess(dob => Candidate.Create("John", "Smith", dob))
                .OnSuccess(Registration.Create);

            registration.IsSuccess.Should().BeTrue();
            registration.Value.HasDomainEvents.Should().BeTrue();

            var flushed = registration.Value.FlushDomainEvents();
            flushed.Count().Should().Be(1);
        }

        [Test]
        public void ShouldNotAllowToCreateRegistrationForNullCandidate()
        {
            var registration = Registration.Create(null);

            registration.IsSuccess.Should().BeFalse();
        }
    }
}