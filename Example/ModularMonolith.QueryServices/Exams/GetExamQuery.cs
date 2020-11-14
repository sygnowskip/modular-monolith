using Hexure.Results;
using MediatR;
using ModularMonolith.Contracts.Exams;

namespace ModularMonolith.QueryServices.Exams
{
    public class GetExamQuery : IRequest<Result<ExamDto>>
    {
        public static Result<GetExamQuery> Create(long examId)
        {
            return Result.Ok(new GetExamQuery());
        }
    }
}