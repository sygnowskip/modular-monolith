using System;

namespace Hexure.EventsPublisher
{
    public class EventsPublisherSettings
    {
        public int BatchSize { get; set; }
        public TimeSpan Delay { get; set; }
    }
}