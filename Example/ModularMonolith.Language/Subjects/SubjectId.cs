using System.Collections.Generic;
using Hexure.Identifiers.Numeric;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Errors;

namespace ModularMonolith.Language.Subjects
{
    public class SubjectId : Identifier
    {
        internal SubjectId(long value) : base(value)
        {
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<SubjectId> Create(long subjectId, ISubjectExistenceValidator subjectExistenceValidator)
        {
            return Result.Create(() => subjectId > 0, DomainErrors.BuildInvalidIdentifier(subjectId))
                .AndEnsure(() => subjectExistenceValidator.Exist(subjectId),
                    DomainErrors.BuildNotFound("Subject", subjectId))
                .OnSuccess(() => new SubjectId(subjectId));
        }
    }
}