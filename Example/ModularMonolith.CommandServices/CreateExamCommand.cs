using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.CommandServices
{
    public class CreateExamCommand : IRequest
    {
        
    }
    
    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand>
    {
        private readonly IExamRepository _examRepository;

        public CreateExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Unit> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            var result = await _examRepository.GetAsync(ExamId.Create(1).Value);
            
            return Unit.Value;
        }
    }
}