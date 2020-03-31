using MediatR;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Contracts.Events
{
    public class PaymentForRegistrationStarted : INotification
    {
        public PaymentForRegistrationStarted(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }
}