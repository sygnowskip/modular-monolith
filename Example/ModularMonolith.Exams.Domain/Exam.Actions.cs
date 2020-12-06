using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Exams.Events;

namespace ModularMonolith.Exams.Domain
{
    public partial class Exam
    {
        public Result<Exam> ChangeCapacity(Capacity capacity)
        {
            if (Capacity == capacity)
                return Result.Ok(this);

            return CanPerform(ExamActions.ChangeCapacity, ExamErrors.Actions.UnableToChangeCapacity.Build())
                .OnSuccess(() =>
                {
                    Capacity = capacity;
                    RaiseEvent(new ExamCapacityChanged(Id, Capacity.Value, _systemTimeProvider.UtcNow));
                    return this;
                });
        }

        public Result<Exam> ChangeRegistrationDates(UtcDate registrationStartDate, UtcDate registrationEndDate)
        {
            if (registrationStartDate.Value >= registrationEndDate.Value)
                return Result.Fail<Exam>(ExamErrors.RegistrationStartDate.HasToBeSetBeforeRegistrationEndDate.Build());

            return ChangeRegistrationStartDate(registrationStartDate)
                .OnSuccess(() => ChangeRegistrationEndDate(registrationEndDate));
        }

        private Result<Exam> ChangeRegistrationStartDate(UtcDate registrationStartDate)
        {
            if (RegistrationStartDate == registrationStartDate)
                return Result.Ok(this);
            
            return CanPerform(ExamActions.ChangeRegistrationStartDate,
                    ExamErrors.RegistrationStartDate.UnableToChange.Build())
                .AndEnsure(() => _systemTimeProvider.UtcNow.Date < registrationStartDate.Value,
                    ExamErrors.RegistrationStartDate.HasToBeSetInTheFuture.Build())
                .OnSuccess(() =>
                {
                    RegistrationStartDate = registrationStartDate;
                    RaiseEvent(new ExamRegistrationStartDateChanged(Id, RegistrationStartDate.Value,
                        _systemTimeProvider.UtcNow));
                    return this;
                });
        }

        private Result<Exam> ChangeRegistrationEndDate(UtcDate registrationEndDate)
        {
            if (RegistrationEndDate == registrationEndDate)
                return Result.Ok(this);

            return CanPerform(ExamActions.ChangeRegistrationEndDate,
                    ExamErrors.RegistrationEndDate.UnableToChange.Build())
                .AndEnsure(() => _systemTimeProvider.UtcNow.Date < registrationEndDate.Value,
                    ExamErrors.RegistrationEndDate.HasToBeSetInTheFuture.Build())
                .AndEnsure(() => registrationEndDate.Value < ExamDateTime.Value.Date, 
                    ExamErrors.RegistrationEndDate.HasToBeSetBeforeExamDate.Build())
                .OnSuccess(() =>
                {
                    RegistrationEndDate = registrationEndDate;
                    RaiseEvent(new ExamRegistrationEndDateChanged(Id, RegistrationEndDate.Value,
                        _systemTimeProvider.UtcNow));
                    return this;
                });
        }
    }

    internal enum ExamActions
    {
        OpenForRegistration,
        CloseRegistration,
        MarkAsDone,
        Delete,
        ChangeCapacity,
        ChangeRegistrationStartDate,
        ChangeRegistrationEndDate
    }
}