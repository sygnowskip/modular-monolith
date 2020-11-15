using Hexure.MediatR;
using Hexure.Results;
using ModularMonolith.Contracts.Exams;

namespace ModularMonolith.CommandServices.Exams
{
    public class EditExamCommand : ICommandRequest
    {
        
        public static Result<EditExamCommand> Create(EditExamRequest request)
        {
            return Result.Ok(new EditExamCommand());
        }
    }
}