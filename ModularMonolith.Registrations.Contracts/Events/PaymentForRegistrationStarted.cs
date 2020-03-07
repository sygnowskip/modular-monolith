using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Contracts.Events
{
    public class PaymentForRegistrationStarted
    {
        public PaymentForRegistrationStarted(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }
}