using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;
using Moq;

namespace ModularMonolith.Exams.Tests.Unit
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
        
        public static LocationId GetLocationId(long locationId)
        {
            var locationExistenceValidatorMock = new Mock<ILocationExistenceValidator>();
            locationExistenceValidatorMock
                .Setup(validator => validator.Exist(It.IsAny<long>()))
                .Returns(true);
            return LocationId.Create(locationId, locationExistenceValidatorMock.Object).Value;
        }
    }
}