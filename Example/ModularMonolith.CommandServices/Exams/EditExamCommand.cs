using Hexure.Results;
using MediatR;
using ModularMonolith.Contracts.Exams;

namespace ModularMonolith.CommandServices.Exams
{
    public class EditExamCommand : IRequest<Result>
    {
        
        public static Result<EditExamCommand> Create(EditExamRequest request)
        {
            return Result.Ok(new EditExamCommand());
        }
    }
}