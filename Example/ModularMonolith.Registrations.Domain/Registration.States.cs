using ModularMonolith.Registrations.Events;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Domain
{
    public partial class Registration
    {
        private void ConfigureStateMachine()
        {
            _stateMachine.Configure(RegistrationStatus.New)
                .Permit(RegistrationActions.Cancel, RegistrationStatus.Cancelled)
                .Permit(RegistrationActions.MarkAsPaid, RegistrationStatus.Paid);

            _stateMachine.Configure(RegistrationStatus.Paid)
                .OnEntry(() => RaiseEvent(new RegistrationPaid(Id, ExternalId, _systemTimeProvider.UtcNow)))
                .Permit(RegistrationActions.Cancel, RegistrationStatus.Cancelled);

            _stateMachine.Configure(RegistrationStatus.Cancelled)
                .OnEntry(() => RaiseEvent(new RegistrationCancelled(Id, ExternalId, _systemTimeProvider.UtcNow)));

        }
    }

    internal enum RegistrationActions
    {
        MarkAsPaid,
        Cancel
    }
}