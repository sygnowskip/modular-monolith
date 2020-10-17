using System;
using Hexure.Events;

namespace ExternalSystem.Events.Locations
{
    public class LocationDeleted : IEvent
    {
        public LocationDeleted(long id, DateTime publishedOn)
        {
            Id = id;
            PublishedOn = publishedOn;
        }

        public long Id { get; }
        public DateTime PublishedOn { get; }
    }
}