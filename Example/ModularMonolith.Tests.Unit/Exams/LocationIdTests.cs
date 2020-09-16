using FluentAssertions;
using ModularMonolith.Exams.Domain.Dependencies;
using ModularMonolith.Exams.Domain.ValueObjects;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Exams
{
    [TestFixture]
    public class LocationIdTests
    {
        private CountryId CountryId;

        [SetUp]
        public void SetUp()
        {
            CountryId = ValueObjectProvider.GetCountryId(10);
        }
        
        [TestCase(-10, false, false)]
        [TestCase(0, false, false)]
        [TestCase(10, false, false)]
        [TestCase(10, true, true)]
        public void ShouldReturnExpectedResult(long locationId, bool exist, bool isSuccess)
        {
            var locationExistenceValidator = new Mock<ILocationExistenceValidator>();
            locationExistenceValidator
                .Setup(validator => validator.Exist(It.IsAny<long>(), It.IsAny<CountryId>()))
                .Returns(exist);

            var locationIdResult = LocationId.Create(locationId, CountryId, locationExistenceValidator.Object);

            locationIdResult.IsSuccess.Should().Be(isSuccess);

            if (isSuccess)
                locationIdResult.Value.Value.Should().Be(locationId);
        }
    }
}