using FluentAssertions;
using ModularMonolith.Language.Subjects;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Exams.Tests.Unit
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