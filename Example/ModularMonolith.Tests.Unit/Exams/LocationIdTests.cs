using FluentAssertions;
using ModularMonolith.Language.Locations;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Exams
{
    [TestFixture]
    public class LocationIdTests
    {
        [TestCase(-10, false, false)]
        [TestCase(0, false, false)]
        [TestCase(10, false, false)]
        [TestCase(10, true, true)]
        public void ShouldReturnExpectedResult(long locationId, bool exist, bool isSuccess)
        {
            var locationExistenceValidator = new Mock<ILocationExistenceValidator>();
            locationExistenceValidator
                .Setup(validator => validator.Exist(It.IsAny<long>()))
                .Returns(exist);

            var locationIdResult = LocationId.Create(locationId, locationExistenceValidator.Object);

            locationIdResult.IsSuccess.Should().Be(isSuccess);

            if (isSuccess)
                locationIdResult.Value.Value.Should().Be(locationId);
        }
    }
}