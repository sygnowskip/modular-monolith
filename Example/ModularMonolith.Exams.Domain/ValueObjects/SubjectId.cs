using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Exams.Domain.Dependencies;

namespace ModularMonolith.Exams.Domain.ValueObjects
{
    public class SubjectId : ValueObject
    {
        private SubjectId(long value)
        {
            Value = value;
        }

        public long Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<SubjectId> Create(long subjectId, ISubjectExistenceValidator subjectExistenceValidator)
        {
            return Result.Create(() => subjectId > 0, SubjectIdErrors.SubjectDoesNotExist.Build())
                .AndEnsure(() => subjectExistenceValidator.Exist(subjectId),
                    SubjectIdErrors.SubjectDoesNotExist.Build())
                .OnSuccess(() => new SubjectId(subjectId));
        }
    }

    public class SubjectIdErrors
    {
        public static readonly Error.ErrorType SubjectDoesNotExist = new Error.ErrorType(nameof(SubjectDoesNotExist), "Subject for provided identifier does not exist");
    }
}