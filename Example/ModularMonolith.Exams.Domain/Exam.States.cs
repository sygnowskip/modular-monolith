using ModularMonolith.Exams.Events;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Domain
{
    public partial class Exam
    {
        protected override void ConfigureStates()
        {
            StateMachine.Configure(ExamStatus.Planned)
                .Permit(ExamActions.Delete, ExamStatus.Deleted)
                .PermitReentry(ExamActions.ChangeCapacity)
                .PermitReentry(ExamActions.ChangeRegistrationStartDate)
                .PermitReentry(ExamActions.ChangeRegistrationEndDate)
                .Permit(ExamActions.OpenForRegistration, ExamStatus.AvailableForRegistration);

            StateMachine.Configure(ExamStatus.AvailableForRegistration)
                .OnEntry(() => RaiseEvent(new ExamAvailable(Id, SystemTimeProvider.UtcNow)))
                .PermitReentry(ExamActions.Book)
                .PermitReentry(ExamActions.Free);

            StateMachine.Configure(ExamStatus.Deleted)
                .OnEntry(() => RaiseEvent(new ExamDeleted(Id, SystemTimeProvider.UtcNow)));
        }
    }

    public enum ExamActions
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