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
                .PermitReentry(ExamActions.ChangeCapacity)
                .PermitReentry(ExamActions.ChangeRegistrationStartDate)
                .PermitReentry(ExamActions.ChangeRegistrationEndDate)
                .PermitIf(ExamActions.OpenForRegistration, ExamStatus.AvailableForRegistration,
                    () => RegistrationStartDate.Value >= _systemTimeProvider.UtcNow);

            _stateMachine.Configure(ExamStatus.AvailableForRegistration)
                .OnEntry(() => RaiseEvent(new ExamAvailable(Id, _systemTimeProvider.UtcNow)))
                .PermitIf(ExamActions.CloseRegistration, ExamStatus.ClosedForRegistration,
                    () => RegistrationEndDate.Value >= _systemTimeProvider.UtcNow)
                .PermitReentry(ExamActions.Book)
                .PermitReentry(ExamActions.Free);

            _stateMachine.Configure(ExamStatus.ClosedForRegistration)
                .OnEntry(() => RaiseEvent(new ExamClosed(Id, _systemTimeProvider.UtcNow)))
                .PermitIf(ExamActions.MarkAsDone, ExamStatus.TookPlace,
                    () => ExamDateTime.Value >= _systemTimeProvider.UtcNow);

            _stateMachine.Configure(ExamStatus.TookPlace)
                .OnEntry(() => RaiseEvent(new ExamTookPlace(Id, _systemTimeProvider.UtcNow)));

            _stateMachine.Configure(ExamStatus.Deleted)
                .OnEntry(() => RaiseEvent(new ExamDeleted(Id, _systemTimeProvider.UtcNow)));
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
        ChangeRegistrationEndDate,
        Book,
        Free
    }
}