using System;
using Hexure.Events;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.Registrations.Events
{
    public class RegistrationCreated : IEvent
    {
        public RegistrationCreated(RegistrationId id, ExternalRegistrationId externalId, Candidate candidate, DateTime publishedOn)
        {
            Id = id;
            ExternalId = externalId;
            Candidate = candidate;
            PublishedOn = publishedOn;
        }

        public RegistrationId Id { get; }
        public ExternalRegistrationId ExternalId { get; }
        public Candidate Candidate { get; }
        public DateTime PublishedOn { get; }
    }
}