using System;
using Hexure.Events;

namespace ExternalSystem.Events.Subjects
{
    public class SubjectAdded : IEvent
    {
        public SubjectAdded(long id, string name, DateTime publishedOn)
        {
            Id = id;
            Name = name;
            PublishedOn = publishedOn;
        }

        public long Id { get; }
        public string Name { get;  }
        public DateTime PublishedOn { get; }
    }
}