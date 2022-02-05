using System.Threading;
using System.Threading.Tasks;
using Hexure.MediatR;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using MediatR;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Exams.Language;
using ModularMonolith.Language;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;

namespace ModularMonolith.CommandServices.Exams
{
    public class CreateExamCommand : ICommandRequest<ExamId>
    {
        private CreateExamCommand(SubjectId subjectId, LocationId locationId, UtcDateTime examDateTime,
            Capacity capacity, UtcDate registrationStartDate, UtcDate registrationEndDate)
        {
            SubjectId = subjectId;
            LocationId = locationId;
            ExamDateTime = examDateTime;
            Capacity = capacity;
            RegistrationStartDate = registrationStartDate;
            RegistrationEndDate = registrationEndDate;
        }

        public SubjectId SubjectId { get; }
        public LocationId LocationId { get; }
        public UtcDateTime ExamDateTime { get; }
        public Capacity Capacity { get; }
        public UtcDate RegistrationStartDate { get; }
        public UtcDate RegistrationEndDate { get; }

        public static Result<CreateExamCommand> Create(CreateExamRequest request,
            ISubjectExistenceValidator subjectExistenceValidator,
            ILocationExistenceValidator locationExistenceValidator)
        {
            var subjectId = SubjectId.Create(request.SubjectId, subjectExistenceValidator)
                .BindErrorsTo(nameof(request.SubjectId));
            var locationId = LocationId.Create(request.LocationId, locationExistenceValidator)
                .BindErrorsTo(nameof(request.LocationId));
            var examDateTime = UtcDateTime.Create(request.ExamDateTime)
                .BindErrorsTo(nameof(request.ExamDateTime));
            var capacity = Capacity.Create(request.Capacity)
                .BindErrorsTo(nameof(request.Capacity));
            var registrationStartDate = UtcDate.Create(request.RegistrationStartDate)
                .BindErrorsTo(nameof(request.RegistrationStartDate));
            var registrationEndDate = UtcDate.Create(request.RegistrationEndDate)
                .BindErrorsTo(nameof(request.RegistrationEndDate));

            return Result.Combine(subjectId, locationId, examDateTime, capacity, registrationStartDate,
                    registrationEndDate)
                .OnSuccess(() => new CreateExamCommand(subjectId.Value, locationId.Value, examDateTime.Value,
                    capacity.Value, registrationStartDate.Value, registrationEndDate.Value));
        }
    }

    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, Result<ExamId>>
    {
        private readonly IExamRepository _examRepository;
        private readonly ISystemTimeProvider _systemTimeProvider;

        public CreateExamCommandHandler(IExamRepository examRepository, ISystemTimeProvider systemTimeProvider)
        {
            _examRepository = examRepository;
            _systemTimeProvider = systemTimeProvider;
        }

        public async Task<Result<ExamId>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            return await Exam.CreateAsync(request.SubjectId, request.LocationId, request.ExamDateTime, request.Capacity,
                    request.RegistrationStartDate, request.RegistrationEndDate, _systemTimeProvider, _examRepository)
                .OnSuccess(exam => exam.Id);
        }
    }
}