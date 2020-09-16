using FluentAssertions;
using ModularMonolith.Exams.Domain.Dependencies;
using ModularMonolith.Exams.Domain.ValueObjects;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Exams
{
    [TestFixture]
    public class SubjectIdTests
    {
        [TestCase(-10, false, false)]
        [TestCase(0, false, false)]
        [TestCase(10, false, false)]
        [TestCase(10, true, true)]
        public void ShouldReturnExpectedResult(long subjectId, bool exist, bool isSuccess)
        {
            var subjectExistenceValidatorMock = new Mock<ISubjectExistenceValidator>();
            subjectExistenceValidatorMock
                .Setup(validator => validator.Exist(It.IsAny<long>()))
                .Returns(exist);

            var subjectIdResult = SubjectId.Create(subjectId, subjectExistenceValidatorMock.Object);

            subjectIdResult.IsSuccess.Should().Be(isSuccess);

            if (isSuccess)
                subjectIdResult.Value.Value.Should().Be(subjectId);
        }
    }
}