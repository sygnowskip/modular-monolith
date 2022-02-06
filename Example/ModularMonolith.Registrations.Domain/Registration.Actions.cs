using Hexure.Results;
using ModularMonolith.Extensions;

namespace ModularMonolith.Registrations.Domain
{
    public partial class Registration
    {
        public Result MarkAsPaid()
        {
            return CheckIfPossible(RegistrationActions.MarkAsPaid,
                RegistrationErrors.Actions.UnableToMarkAsPaid.Build());
        }

        public Result Cancel()
        {
            return CheckIfPossible(RegistrationActions.Cancel,
                RegistrationErrors.Actions.UnableToCancel.Build());
        }
    }
}