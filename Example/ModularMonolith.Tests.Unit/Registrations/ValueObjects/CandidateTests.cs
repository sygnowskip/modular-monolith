using System;
using FluentAssertions;
using ModularMonolith.Registrations.ValueObjects;
using ModularMonolith.Time;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Registrations.ValueObjects
{
    [TestFixture]
    public class CandidateTests
    {
        private readonly Mock<ISystemTimeProvider> _systemTimeProviderMock = new Mock<ISystemTimeProvider>();
        private readonly DateOfBirth _dateOfBirth;

        public CandidateTests()
        {
            _systemTimeProviderMock.Setup(provider => provider.UtcNow)
                .Returns(() => new DateTime(2020, 03, 01));
            _dateOfBirth = DateOfBirth.Create(new DateTime(1980, 01, 01), _systemTimeProviderMock.Object).Value;
        }

        [TestCase("John", "Smith", true)]
        [TestCase(null, "Smith", false)]
        [TestCase("", "Smith", false)]
        [TestCase("John", null, false)]
        [TestCase("John", "", false)]
        [TestCase(null, null, false)]
        [TestCase("", "", false)]
        public void ShouldCreateCanddiateWithExpectedResult(string firstName, string lastName, bool expectedResult)
        {
            var candidate = Candidate.Create(firstName, lastName, _dateOfBirth);

            candidate.IsSuccess.Should().Be(expectedResult);
        }

    }
}