using System.Threading.Tasks;
using Hexure;
using Hexure.Events;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Exams.Events;
using ModularMonolith.Exams.Language;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;
using Stateless;

namespace ModularMonolith.Exams.Domain
{
    public partial class Exam
    {
        public static Task<Result<Exam>> CreateAsync(SubjectId subjectId, LocationId locationId, UtcDateTime examDateTime,
            Capacity capacity, UtcDate registrationStartDate, UtcDate registrationEndDate, 
            ISystemTimeProvider systemTimeProvider, IExamRepository examRepository)
        {
            return Result.Create(() => systemTimeProvider.UtcNow < registrationStartDate.Value,
                    ExamCreationErrors.RegistrationStartDateHasToBeInTheFuture.Build())
                .AndEnsure(() => registrationStartDate.Value < registrationEndDate.Value,
                    ExamCreationErrors.RegistrationEndDateHasToBeAfterRegistrationStartDate.Build())
                .AndEnsure(() => registrationEndDate.Value < examDateTime.Value.Date,
                    ExamCreationErrors.RegistrationEndDateHasToBeBeforeExamDate.Build())
                .OnSuccess(() => new Exam(subjectId, locationId, examDateTime, capacity, registrationStartDate,
                    registrationEndDate, systemTimeProvider))
                .OnSuccess(examRepository.SaveAsync)
                .OnSuccess(exam => exam.RaiseEvent(new ExamPlanned(exam.Id, systemTimeProvider.UtcNow)));
        }
    }

    public static class ExamCreationErrors
    {
        public static readonly Error.ErrorType RegistrationStartDateHasToBeInTheFuture =
            new Error.ErrorType(nameof(RegistrationStartDateHasToBeInTheFuture), "Registration start date has to be in the future");
        
        public static readonly Error.ErrorType RegistrationEndDateHasToBeAfterRegistrationStartDate =
            new Error.ErrorType(nameof(RegistrationEndDateHasToBeAfterRegistrationStartDate), "Registration end date has to be after registration start date");
        
        public static readonly Error.ErrorType RegistrationEndDateHasToBeBeforeExamDate =
            new Error.ErrorType(nameof(RegistrationEndDateHasToBeBeforeExamDate), "Registration end date has to be before exam date"); 
    }
}