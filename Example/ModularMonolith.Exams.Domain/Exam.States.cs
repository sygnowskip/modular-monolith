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
                .Permit(ExamActions.OpenForRegistration, ExamStatus.AvailableForRegistration);

            _stateMachine.Configure(ExamStatus.AvailableForRegistration)
                .OnEntry(() => RaiseEvent(new ExamAvailable(Id, _systemTimeProvider.UtcNow)))
                .PermitReentry(ExamActions.Book)
                .PermitReentry(ExamActions.Free);

            _stateMachine.Configure(ExamStatus.Deleted)
                .OnEntry(() => RaiseEvent(new ExamDeleted(Id, _systemTimeProvider.UtcNow)));
        }
    }

    internal enum ExamActions
    {
        OpenForRegistration,
        Delete,
        ChangeCapacity,
        ChangeRegistrationStartDate,
        ChangeRegistrationEndDate,
        Book,
        Free
    }
}