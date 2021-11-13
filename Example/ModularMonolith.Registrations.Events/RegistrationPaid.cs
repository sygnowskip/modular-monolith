using System;
using Hexure.Events;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Events
{
    public class RegistrationPaid : IEvent
    {
        public RegistrationPaid(RegistrationId id, DateTime publishedOn)
        {
            Id = id;
            PublishedOn = publishedOn;
        }

        public RegistrationId Id { get; }
        public DateTime PublishedOn { get; }
    }
}