using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using MediatR;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.CommandServices.Exams
{
    public class CreateExamCommand : IRequest<Result<ExamId>>
    {
        public static Result<CreateExamCommand> Create(CreateExamRequest request)
        {
            return Result.Ok(new CreateExamCommand());
        }
    }
    
    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, Result<ExamId>>
    {
        private readonly IExamRepository _examRepository;

        public CreateExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Result<ExamId>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            var result = await _examRepository.GetAsync(ExamId.Create(1).Value);
            
            return  Result.Ok(ExamId.Create(12).Value);
        }
    }
}