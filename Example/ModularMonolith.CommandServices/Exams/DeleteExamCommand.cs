using Hexure.Results;
using MediatR;

namespace ModularMonolith.CommandServices.Exams
{
    public class DeleteExamCommand : IRequest<Result>
    {
        
        public static Result<DeleteExamCommand> Create(long examId)
        {
            return Result.Ok(new DeleteExamCommand());
        }
    }
}