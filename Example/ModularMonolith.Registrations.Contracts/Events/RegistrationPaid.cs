using System;
using Hexure.Events;
using MediatR;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Contracts.Events
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