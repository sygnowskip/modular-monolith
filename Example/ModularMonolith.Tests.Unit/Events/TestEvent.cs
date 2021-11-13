using System;
using Hexure.Events;

namespace ModularMonolith.Tests.Unit.Events
{

    public class TestEvent : IEvent
    {
        public TestEvent(Guid id, DateTime publishedOn)
        {
            Id = id;
            PublishedOn = publishedOn;
        }

        public Guid Id { get; }
        public DateTime PublishedOn { get; }
    }
}