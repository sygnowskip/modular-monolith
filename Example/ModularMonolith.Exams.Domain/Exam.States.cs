using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Exams.Events;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Domain
{
    public partial class Exam
    {
        private void ConfigureStateMachine()
        {
            _stateMachine.Configure(ExamStatus.Planned)
                .Permit(ExamActions.Delete, ExamStatus.Deleted)
                .PermitIf(ExamActions.OpenForRegistration, ExamStatus.AvailableForRegistration,
                    () => RegistrationStartDate.Value >= _systemTimeProvider.UtcNow);

            _stateMachine.Configure(ExamStatus.AvailableForRegistration)
                .OnEntry(() => RaiseEvent(new ExamAvailable(Id, _systemTimeProvider.UtcNow)))
                .PermitIf(ExamActions.CloseRegistration, ExamStatus.ClosedForRegistration,
                    () => RegistrationEndDate.Value >= _systemTimeProvider.UtcNow);

            _stateMachine.Configure(ExamStatus.ClosedForRegistration)
                .OnEntry(() => RaiseEvent(new ExamClosed(Id, _systemTimeProvider.UtcNow)))
                .PermitIf(ExamActions.MarkAsDone, ExamStatus.TookPlace,
                    () => ExamDateTime.Value >= _systemTimeProvider.UtcNow);

            _stateMachine.Configure(ExamStatus.TookPlace)
                .OnEntry(() => RaiseEvent(new ExamTookPlace(Id, _systemTimeProvider.UtcNow)));
        }

        public Result OpenForRegistration()
        {
            return PerformIfPossible(ExamActions.OpenForRegistration,
                ExamErrors.Actions.UnableToOpenForRegistration.Build());
        }

        public Result CloseRegistration()
        {
            return PerformIfPossible(ExamActions.CloseRegistration,
                ExamErrors.Actions.UnableToCloseRegistration.Build());
        }

        public Result MarkAsDone()
        {
            return PerformIfPossible(ExamActions.MarkAsDone, ExamErrors.Actions.UnableToMarkAsDone.Build());
        }

        public Result Delete()
        {
            return PerformIfPossible(ExamActions.Delete, ExamErrors.Actions.UnableToDelete.Build());
        }

        private Result CanPerform(ExamActions action, Error error)
        {
            if (!_stateMachine.CanFire(action))
                return Result.Fail(error);

            return Result.Ok();
        }

        private Result PerformIfPossible(ExamActions action, Error error)
        {
            return CanPerform(action, error)
                .OnSuccess(() => _stateMachine.Fire(action));
        }
    }
}