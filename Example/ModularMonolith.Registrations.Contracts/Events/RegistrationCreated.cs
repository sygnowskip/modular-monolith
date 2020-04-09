using System;
using Hexure.Events;
using MediatR;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.Registrations.Contracts.Events
{
    public class RegistrationCreated : IEvent
    {
        public RegistrationCreated(RegistrationId id, Candidate candidate, DateTime publishedOn)
        {
            Id = id;
            Candidate = candidate;
            PublishedOn = publishedOn;
        }

        public RegistrationId Id { get; }
        public Candidate Candidate { get; }
        public DateTime PublishedOn { get; }
    }
}