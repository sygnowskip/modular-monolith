using Hexure.Identifiers.Numeric;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Errors;
using ModularMonolith.Exams.Language.Validators;
using Newtonsoft.Json;

namespace ModularMonolith.Exams.Language
{
    public sealed class ExamId : Identifier
    {
        [JsonConstructor]
        internal ExamId(long value) : base(value)
        {
        }

        public static Result<ExamId> Create(long examId, IExamExistenceValidator examExistenceValidator)
        {
            return Result.Create(() => examId > 0, DomainErrors.BuildInvalidIdentifier(examId))
                .AndEnsure(() => examExistenceValidator.Exist(examId), DomainErrors.BuildAggregateNotFound("Exam", examId))
                .OnSuccess(() => new ExamId(examId));
        }
    }
}