using System;
using Hexure.Events;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Events
{
    public class RegistrationCancelled : IEvent
    {
        public RegistrationCancelled(RegistrationId id, ExternalRegistrationId externalId, DateTime publishedOn)
        {
            Id = id;
            ExternalId = externalId;
            PublishedOn = publishedOn;
        }

        public RegistrationId Id { get; }
        public ExternalRegistrationId ExternalId { get; }
        public DateTime PublishedOn { get; }
    }
}