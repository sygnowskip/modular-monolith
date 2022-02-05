using System.Threading;
using System.Threading.Tasks;
using Hexure.MediatR;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language;
using ModularMonolith.Exams.Language.Validators;

namespace ModularMonolith.CommandServices.Exams
{
    public class OpenExamForRegistrationCommand : ICommandRequest
    {
        public OpenExamForRegistrationCommand(ExamId examId)
        {
            ExamId = examId;
        }

        public ExamId ExamId { get; }
        
        public static Result<OpenExamForRegistrationCommand> Create(long examId, IExamExistenceValidator examExistenceValidator)
        {
            return ExamId.Create(examId, examExistenceValidator)
                .OnSuccess(id => new OpenExamForRegistrationCommand(id));
        }
    }
    
    public class OpenExamForRegistrationCommandHandler : IRequestHandler<OpenExamForRegistrationCommand, Result>
    {
        private readonly IExamRepository _examRepository;

        public OpenExamForRegistrationCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Result> Handle(OpenExamForRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetAsync(request.ExamId)
                .OnSuccess(exam => exam.OpenForRegistration());
        }
    }
}