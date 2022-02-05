using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hexure.Results;
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
        private readonly Mock<IRegistrationRepository> _registrationRepositoryMock;

        public RegistrationTests()
        {
            _systemTimeProviderMock = new Mock<ISystemTimeProvider>();
            _systemTimeProviderMock
                .Setup(provider => provider.UtcNow)
                .Returns(new DateTime(2020, 04, 01));

            _registrationRepositoryMock = new Mock<IRegistrationRepository>();
            _registrationRepositoryMock.Setup(r => r.SaveAsync(It.IsAny<Registration>()))
                .Returns<Registration>(registration => Task.FromResult(Result.Ok(registration)));
        }

        [Test]
        public async Task ShouldCreateRegistration()
        {
            var registration = await DateOfBirth.Create(new DateTime(1980, 01, 01), _systemTimeProviderMock.Object)
                .OnSuccess(dob => Candidate.Create("John", "Smith", dob))
                .OnSuccess(candidate => Registration.CreateAsync(new ExternalRegistrationId(),new ExamId(10), new OrderId(10), candidate,  
                    _systemTimeProviderMock.Object, _registrationRepositoryMock.Object));

            registration.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task ShouldAddEventOnRegistrationCreation()
        {
            var registration = await DateOfBirth.Create(new DateTime(1980, 01, 01), _systemTimeProviderMock.Object)
                .OnSuccess(dob => Candidate.Create("John", "Smith", dob))
                .OnSuccess(candidate => Registration.CreateAsync(new ExternalRegistrationId(),new ExamId(10), new OrderId(10), candidate, 
                    _systemTimeProviderMock.Object, _registrationRepositoryMock.Object));

            registration.IsSuccess.Should().BeTrue();
            registration.Value.HasDomainEvents.Should().BeTrue();

            var flushed = registration.Value.FlushDomainEvents();
            flushed.Count().Should().Be(1);
        }

        [Test]
        public async Task ShouldNotAllowToCreateRegistrationForNullCandidate()
        {
            var registration = await Registration.CreateAsync(new ExternalRegistrationId(),new ExamId(10), new OrderId(10), null, 
                _systemTimeProviderMock.Object, _registrationRepositoryMock.Object);

            registration.IsSuccess.Should().BeFalse();
        }
    }
}