using ModularMonolith.Registrations.Events;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Domain
{
    public partial class Registration
    {
        protected override void ConfigureStates()
        {
            StateMachine.Configure(RegistrationStatus.New)
                .Permit(RegistrationActions.Cancel, RegistrationStatus.Cancelled)
                .Permit(RegistrationActions.MarkAsPaid, RegistrationStatus.Paid);

            StateMachine.Configure(RegistrationStatus.Paid)
                .OnEntry(() => RaiseEvent(new RegistrationPaid(Id, ExternalId, SystemTimeProvider.UtcNow)))
                .Permit(RegistrationActions.Cancel, RegistrationStatus.Cancelled);

            StateMachine.Configure(RegistrationStatus.Cancelled)
                .OnEntry(() => RaiseEvent(new RegistrationCancelled(Id, ExternalId, SystemTimeProvider.UtcNow)));

        }
    }

    public enum RegistrationActions
    {
        MarkAsPaid,
        Cancel
    }
}