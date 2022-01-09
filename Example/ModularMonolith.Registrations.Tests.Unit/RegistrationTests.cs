using System;
using System.Linq;
using FluentAssertions;
using Hexure.Results.Extensions;
using Hexure.Time;
using ModularMonolith.Exams.Language;
using ModularMonolith.Orders.Language;
using ModularMonolith.Registrations.Domain;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Registrations.Tests.Unit
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
                .OnSuccess(candidate => Registration.Create(new ExternalRegistrationId(),new ExamId(10), new OrderId(10), candidate,  _systemTimeProviderMock.Object));

            registration.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ShouldAddEventOnRegistrationCreation()
        {
            var registration = DateOfBirth.Create(new DateTime(1980, 01, 01), _systemTimeProviderMock.Object)
                .OnSuccess(dob => Candidate.Create("John", "Smith", dob))
                .OnSuccess(candidate => Registration.Create(new ExternalRegistrationId(),new ExamId(10), new OrderId(10), candidate, _systemTimeProviderMock.Object));

            registration.IsSuccess.Should().BeTrue();
            registration.Value.HasDomainEvents.Should().BeTrue();

            var flushed = registration.Value.FlushDomainEvents();
            flushed.Count().Should().Be(1);
        }

        [Test]
        public void ShouldNotAllowToCreateRegistrationForNullCandidate()
        {
            var registration = Registration.Create(new ExternalRegistrationId(),new ExamId(10), new OrderId(10), null, _systemTimeProviderMock.Object);

            registration.IsSuccess.Should().BeFalse();
        }
    }
}