using MediatR;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.Registrations.Contracts.Events
{
    public class RegistrationCreated : INotification
    {
        public RegistrationCreated(RegistrationId id, Candidate candidate)
        {
            Id = id;
            Candidate = candidate;
        }

        public RegistrationId Id { get; }
        public Candidate Candidate { get; }
    }
}