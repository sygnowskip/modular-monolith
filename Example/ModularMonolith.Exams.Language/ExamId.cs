using Hexure.Identifiers.Numeric;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Exams.Language
{
    public class ExamId : Identifier
    {
        protected ExamId(long value) : base(value)
        {
        }

        public static Result<ExamId> Create(long examId)
        {
            return Result.Create(() => examId > 0, ExamIdErrors.InvalidExamIdentifier.Build())
                .OnSuccess(() => new ExamId(examId));
        }
    }

    public class ExamIdErrors
    {
        public static readonly Error.ErrorType InvalidExamIdentifier = new Error.ErrorType(nameof(InvalidExamIdentifier), "Invalid exam identifier");
    }
}