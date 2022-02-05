using System.Collections.Generic;
using Hexure.Results;
using ModularMonolith.Language;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;

namespace ModularMonolith.Registrations.Domain.ValueObject
{
    public class RegistrationExamInfo : Hexure.Results.ValueObject
    {
        public RegistrationExamInfo(SubjectId subjectId, LocationId locationId, UtcDateTime examDateTime)
        {
            SubjectId = subjectId;
            LocationId = locationId;
            ExamDateTime = examDateTime;
        }

        public SubjectId SubjectId { get; private set; }
        public LocationId LocationId { get; private set; }
        public UtcDateTime ExamDateTime { get; private set; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return SubjectId;
            yield return LocationId;
            yield return ExamDateTime;
        }

        public static Result<RegistrationExamInfo> Create(SubjectId subjectId, LocationId locationId, UtcDateTime examDateTime)
        {
            return Result.Ok(new RegistrationExamInfo(subjectId, locationId, examDateTime));
        }
    }
}