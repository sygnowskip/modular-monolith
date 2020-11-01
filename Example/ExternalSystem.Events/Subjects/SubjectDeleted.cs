using System;
using Hexure.Events;

namespace ExternalSystem.Events.Subjects
{
    public class SubjectDeleted : IEvent
    {
        public SubjectDeleted(long id, DateTime publishedOn)
        {
            Id = id;
            PublishedOn = publishedOn;
        }

        public long Id { get; }
        public DateTime PublishedOn { get; }
    }
}