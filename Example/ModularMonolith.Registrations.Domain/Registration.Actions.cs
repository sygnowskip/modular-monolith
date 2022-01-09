using Hexure.Results;
using ModularMonolith.Extensions;

namespace ModularMonolith.Registrations.Domain
{
    public partial class Registration
    {
        public Result MarkAsPaid()
        {
            return _stateMachine.PerformIfPossible(RegistrationActions.MarkAsPaid,
                RegistrationErrors.Actions.UnableToMarkAsPaid.Build());
        }

        public Result Cancel()
        {
            return _stateMachine.PerformIfPossible(RegistrationActions.Cancel,
                RegistrationErrors.Actions.UnableToCancel.Build());
        }
    }
}