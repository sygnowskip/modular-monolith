using System;

namespace Hexure.EventsPublisher
{
    public static class EventsPublisherFactory
    {
        public static EventsPublisherBuilder CreatePublisher(int defaultBatchSize, TimeSpan defaultDelay)
        {
            return new EventsPublisherBuilder(defaultBatchSize, defaultDelay);
        }
    }
}
