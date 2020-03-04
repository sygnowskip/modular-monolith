using MediatR;

namespace ModularMonolith.Registrations.Contracts.Events
{
    public class RegistrationPaid : INotification
    {
        public RegistrationPaid(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }
}