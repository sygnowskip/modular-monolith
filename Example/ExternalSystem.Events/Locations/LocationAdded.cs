using System;
using Hexure.Events;

namespace ExternalSystem.Events.Locations
{
    public class LocationAdded : IEvent
    {
        public LocationAdded(long id, string name, DateTime publishedOn)
        {
            Id = id;
            Name = name;
            PublishedOn = publishedOn;
        }

        public long Id { get; }
        public string Name { get; }
        public DateTime PublishedOn { get; }
    }
}