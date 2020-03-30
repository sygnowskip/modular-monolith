using System;
using System.Collections.Generic;
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
        private static readonly Mock<ISystemTimeProvider> SystemTimeProviderMock = new Mock<ISystemTimeProvider>();
        private static readonly DateOfBirth DateOfBirth;

        static CandidateTests()
        {
            SystemTimeProviderMock.Setup(provider => provider.UtcNow)
                .Returns(() => new DateTime(2020, 03, 01));
            DateOfBirth = DateOfBirth.Create(new DateTime(1980, 01, 01), SystemTimeProviderMock.Object).Value;
        }

        [TestCaseSource(nameof(ShouldCreateCandidateWithExpectedResultCases))]
        public void ShouldCreateCandidateWithExpectedResult(string firstName, string lastName, DateOfBirth dateOfBirth, bool expectedResult)
        {
            var candidate = Candidate.Create(firstName, lastName, dateOfBirth);

            candidate.IsSuccess.Should().Be(expectedResult);
        }

        private static IEnumerable<TestCaseData> ShouldCreateCandidateWithExpectedResultCases
        {
            get
            {
                yield return new TestCaseData("John", "Smith", DateOfBirth, true);
                yield return new TestCaseData("John", "Smith", null, false);
                yield return new TestCaseData("John", "", null, false);
                yield return new TestCaseData("John", null, DateOfBirth, false);
                yield return new TestCaseData("", "Smith", null, false);
                yield return new TestCaseData(null, "", null, false);
                yield return new TestCaseData(null, "Smith", DateOfBirth, false);
                yield return new TestCaseData(null, null, null, false);
            }
        }
    }
}