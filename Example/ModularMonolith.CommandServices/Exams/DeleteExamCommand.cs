using System.Threading;
using System.Threading.Tasks;
using Hexure.MediatR;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Errors;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language;
using ModularMonolith.Exams.Language.Validators;

namespace ModularMonolith.CommandServices.Exams
{
    public class DeleteExamCommand : ICommandRequest
    {
        private DeleteExamCommand(ExamId examId)
        {
            ExamId = examId;
        }

        public ExamId ExamId { get; }
        
        public static Result<DeleteExamCommand> Create(long examId, IExamExistenceValidator examExistenceValidator)
        {
            return ExamId.Create(examId, examExistenceValidator)
                .OnSuccess(id => new DeleteExamCommand(id));
        }
    }

    public class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand, Result>
    {
        private readonly IExamRepository _examRepository;

        public DeleteExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Result> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetAsync(request.ExamId)
                .ToResult(DomainErrors.BuildNotFound(nameof(Exam), request.ExamId.Value))
                .OnSuccess(exam => exam.Delete());
        }
    }
}