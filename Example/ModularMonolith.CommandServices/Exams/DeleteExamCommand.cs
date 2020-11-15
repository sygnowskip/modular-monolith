using Hexure.MediatR;
using Hexure.Results;

namespace ModularMonolith.CommandServices.Exams
{
    public class DeleteExamCommand : ICommandRequest
    {
        
        public static Result<DeleteExamCommand> Create(long examId)
        {
            return Result.Ok(new DeleteExamCommand());
        }
    }
}