using ModularMonolith.Exams.Domain.Dependencies;
using ModularMonolith.Exams.Domain.ValueObjects;
using Moq;

namespace ModularMonolith.Tests.Unit
{
    public static class ValueObjectProvider
    {
        public static SubjectId GetSubjectId(long subjectId)
        {
            var subjectExistenceValidatorMock = new Mock<ISubjectExistenceValidator>();
            subjectExistenceValidatorMock
                .Setup(validator => validator.Exist(It.IsAny<long>()))
                .Returns(true);
            return SubjectId.Create(subjectId, subjectExistenceValidatorMock.Object).Value;
        }
        
        public static CountryId GetCountryId(long countryId)
        {
            var countryExistenceValidatorMock = new Mock<ICountryExistenceValidator>();
            countryExistenceValidatorMock
                .Setup(validator => validator.Exist(It.IsAny<long>()))
                .Returns(true);
            return CountryId.Create(countryId, countryExistenceValidatorMock.Object).Value;
        }
        
        public static LocationId GetLocationId(long locationId)
        {
            var locationExistenceValidatorMock = new Mock<ILocationExistenceValidator>();
            locationExistenceValidatorMock
                .Setup(validator => validator.Exist(It.IsAny<long>(), It.IsAny<CountryId>()))
                .Returns(true);
            return LocationId.Create(locationId, GetCountryId(locationId), locationExistenceValidatorMock.Object).Value;
        }
    }
}