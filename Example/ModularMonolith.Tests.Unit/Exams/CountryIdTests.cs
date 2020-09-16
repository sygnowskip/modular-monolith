using FluentAssertions;
using ModularMonolith.Exams.Domain.Dependencies;
using ModularMonolith.Exams.Domain.ValueObjects;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Exams
{
    [TestFixture]
    public class CountryIdTests
    {
        [TestCase(-10, false, false)]
        [TestCase(0, false, false)]
        [TestCase(10, false, false)]
        [TestCase(10, true, true)]
        public void ShouldReturnExpectedResult(long countryId, bool exist, bool isSuccess)
        {
            var countryExistenceValidatorMock = new Mock<ICountryExistenceValidator>();
            countryExistenceValidatorMock
                .Setup(validator => validator.Exist(It.IsAny<long>()))
                .Returns(exist);

            var countryIdResult = CountryId.Create(countryId, countryExistenceValidatorMock.Object);

            countryIdResult.IsSuccess.Should().Be(isSuccess);

            if (isSuccess)
                countryIdResult.Value.Value.Should().Be(countryId);
        }
    }
}