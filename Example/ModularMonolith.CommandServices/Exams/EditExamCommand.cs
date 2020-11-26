using System.Threading;
using System.Threading.Tasks;
using Hexure.MediatR;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Exams.Language;
using ModularMonolith.Exams.Language.Validators;

namespace ModularMonolith.CommandServices.Exams
{
    public class EditExamCommand : ICommandRequest
    {
        public ExamId ExamId { get; }
        public Capacity Capacity { get; }
        public UtcDate RegistrationStartDate { get; }
        public UtcDate RegistrationEndDate { get; }

        private EditExamCommand(ExamId examId, Capacity capacity, UtcDate registrationStartDate, UtcDate registrationEndDate)
        {
            ExamId = examId;
            Capacity = capacity;
            RegistrationStartDate = registrationStartDate;
            RegistrationEndDate = registrationEndDate;
        }

        public static Result<EditExamCommand> Create(long examId, EditExamRequest request, IExamExistenceValidator examExistenceValidator)
        {
            var id = ExamId.Create(examId, examExistenceValidator);
            var capacity = Capacity.Create(request.Capacity)
                .BindErrorsTo(nameof(request.Capacity));
            var registrationStartDate = UtcDate.Create(request.RegistrationStartDate)
                .BindErrorsTo(nameof(request.RegistrationStartDate));
            var registrationEndDate = UtcDate.Create(request.RegistrationEndDate)
                .BindErrorsTo(nameof(request.RegistrationEndDate));
            
            return Result.Combine(id, capacity, registrationStartDate, registrationEndDate)
                .OnSuccess(() => new EditExamCommand(id.Value, capacity.Value, registrationStartDate.Value, registrationEndDate.Value));
        }
    }

    public class EditExamCommandHandler : IRequestHandler<EditExamCommand, Result>
    {
        private readonly IExamRepository _examRepository;

        public EditExamCommandHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Result> Handle(EditExamCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.GetAsync(request.ExamId)
                .OnSuccess(exam => exam.ChangeCapacity(request.Capacity))
                .OnSuccess(exam => exam.ChangeRegistrationDates(request.RegistrationStartDate, request.RegistrationEndDate));
        }
    }
}