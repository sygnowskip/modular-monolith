﻿using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Exams.Events;
using ModularMonolith.Extensions;
using ModularMonolith.Language;

namespace ModularMonolith.Exams.Domain
{
    public partial class Exam
    {
        public Result<Exam> ChangeCapacity(Capacity capacity)
        {
            if (Capacity == capacity)
                return Result.Ok(this);

            return CheckIfPossible(ExamActions.ChangeCapacity, ExamErrors.Actions.UnableToChangeCapacity.Build())
                .OnSuccess(() =>
                {
                    Capacity = capacity;
                    RaiseEvent(new ExamCapacityChanged(Id, Capacity.Value, SystemTimeProvider.UtcNow));
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

        public Result OpenForRegistration()
        {
            return CheckIfPossible(ExamActions.OpenForRegistration,
                ExamErrors.Actions.UnableToOpenForRegistration.Build());
        }

        public Result Delete()
        {
            return CheckIfPossible(ExamActions.Delete, ExamErrors.Actions.UnableToDelete.Build());
        }

        public Result Book()
        {
            return Result.Create(() => Booked.Value < Capacity.Value, ExamErrors.NoMoreCapacityAvailable.Build())
                .OnSuccess(() => Booked.Increase())
                .OnSuccess(newBooked => Booked = newBooked);
        }

        public Result Free()
        {
            return Booked.Decrease()
                .OnSuccess(newBooked => Booked = newBooked);
        }

        private Result<Exam> ChangeRegistrationStartDate(UtcDate registrationStartDate)
        {
            if (RegistrationStartDate == registrationStartDate)
                return Result.Ok(this);
            
            return CheckIfPossible(ExamActions.ChangeRegistrationStartDate,
                    ExamErrors.RegistrationStartDate.UnableToChange.Build())
                .AndEnsure(() => SystemTimeProvider.UtcNow.Date < registrationStartDate.Value,
                    ExamErrors.RegistrationStartDate.HasToBeSetInTheFuture.Build())
                .OnSuccess(() =>
                {
                    RegistrationStartDate = registrationStartDate;
                    RaiseEvent(new ExamRegistrationStartDateChanged(Id, RegistrationStartDate.Value,
                        SystemTimeProvider.UtcNow));
                    return this;
                });
        }

        private Result<Exam> ChangeRegistrationEndDate(UtcDate registrationEndDate)
        {
            if (RegistrationEndDate == registrationEndDate)
                return Result.Ok(this);

            return CheckIfPossible(ExamActions.ChangeRegistrationEndDate,
                    ExamErrors.RegistrationEndDate.UnableToChange.Build())
                .AndEnsure(() => SystemTimeProvider.UtcNow.Date < registrationEndDate.Value,
                    ExamErrors.RegistrationEndDate.HasToBeSetInTheFuture.Build())
                .AndEnsure(() => registrationEndDate.Value < ExamDateTime.Value.Date, 
                    ExamErrors.RegistrationEndDate.HasToBeSetBeforeExamDate.Build())
                .OnSuccess(() =>
                {
                    RegistrationEndDate = registrationEndDate;
                    RaiseEvent(new ExamRegistrationEndDateChanged(Id, RegistrationEndDate.Value,
                        SystemTimeProvider.UtcNow));
                    return this;
                });
        }
    }
}