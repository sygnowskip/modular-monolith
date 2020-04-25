namespace Hexure.EventsConsumer
{
    public class EventsConsumerFactory
    {
        public static EventsConsumerBuilder Create()
        {
            return new EventsConsumerBuilder();
        }
    }
}