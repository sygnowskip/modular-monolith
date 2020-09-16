using System.Threading.Tasks;
using Hexure;
using Hexure.Events;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Exams.Events;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Domain
{
    public class Exam : Entity, IAggregateRoot<ExamId>
    {
        public Exam(SubjectId subjectId, LocationId locationId, UtcDateTime examDateTime, Capacity capacity,
            UtcDate registrationStartDate, UtcDate registrationEndDate)
        {
            LocationId = locationId;
            Capacity = capacity;
            SubjectId = subjectId;
            RegistrationStartDate = registrationStartDate;
            RegistrationEndDate = registrationEndDate;
            ExamDateTime = examDateTime;
            Status = ExamStatus.Planned;
        }

        public ExamId Id { get; }
        public LocationId LocationId { get; }
        public Capacity Capacity { get; }
        public SubjectId SubjectId { get; }

        public UtcDate RegistrationStartDate { get; }
        public UtcDate RegistrationEndDate { get; }

        public UtcDateTime ExamDateTime { get; }

        public ExamStatus Status { get; }

        public static Task<Result<Exam>> CreateAsync(SubjectId subjectId, LocationId locationId, UtcDateTime examDateTime,
            Capacity capacity, UtcDate registrationStartDate, UtcDate registrationEndDate, 
            ISystemTimeProvider systemTimeProvider, IExamRepository examRepository)
        {
            return Result.Create(() => systemTimeProvider.UtcNow < registrationStartDate.Value,
                    ExamErrors.RegistrationStartDateHasToBeInTheFuture.Build())
                .AndEnsure(() => registrationStartDate.Value < registrationEndDate.Value,
                    ExamErrors.RegistrationEndDateHasToBeAfterRegistrationStartDate.Build())
                .AndEnsure(() => registrationEndDate.Value < examDateTime.Value.Date,
                    ExamErrors.RegistrationEndDateHasToBeBeforeExamDate.Build())
                .OnSuccess(() => new Exam(subjectId, locationId, examDateTime, capacity, registrationStartDate,
                    registrationEndDate))
                .OnSuccess(examRepository.SaveAsync)
                .OnSuccess(exam => exam.RaiseEvent(new ExamPlanned(exam.Id, systemTimeProvider.UtcNow)));
        }
    }

    public class ExamErrors
    {
        public static readonly Error.ErrorType RegistrationStartDateHasToBeInTheFuture =
            new Error.ErrorType(nameof(RegistrationStartDateHasToBeInTheFuture), "Registration start date has to be in the future");
        
        public static readonly Error.ErrorType RegistrationEndDateHasToBeAfterRegistrationStartDate =
            new Error.ErrorType(nameof(RegistrationEndDateHasToBeAfterRegistrationStartDate), "Registration end date has to be after registration start date");
        
        public static readonly Error.ErrorType RegistrationEndDateHasToBeBeforeExamDate =
            new Error.ErrorType(nameof(RegistrationEndDateHasToBeBeforeExamDate), "Registration end date has to be before exam date"); 
    }
}