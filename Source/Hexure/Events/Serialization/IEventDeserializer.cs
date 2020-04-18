using Hexure.Results;
using Hexure.Results.Extensions;
using Newtonsoft.Json;

namespace Hexure.Events.Serialization
{
    public static class EventDeserializerErrors
    {
        public static Error.ErrorType UnableToFindTypeForEvent = new Error.ErrorType(nameof(UnableToFindTypeForEvent),
            "Unable to find type for event {0} from namespace {1}");

        public static Error.ErrorType UnableToDeserializeNullEvent = new Error.ErrorType(nameof(UnableToDeserializeNullEvent),
            $"Unable to deserialize null event");

        public static Error.ErrorType UnableToDeserializeEvent = new Error.ErrorType(nameof(UnableToDeserializeEvent),
            $"Unable to deserialize event");
    }

    public interface IEventDeserializer
    {
        Result<IEvent> Deserialize(SerializedEvent @event);
    }

    public class EventDeserializer : IEventDeserializer
    {
        private readonly IEventTypeProvider _eventTypeProvider;

        public EventDeserializer(IEventTypeProvider eventTypeProvider)
        {
            _eventTypeProvider = eventTypeProvider;
        }

        public Result<IEvent> Deserialize(SerializedEvent @event)
        {
            if (@event == null)
                return Result.Fail<IEvent>(EventDeserializerErrors.UnableToDeserializeNullEvent.Build());

            var eventType = _eventTypeProvider.GetType(@event.Namespace, @event.Type);
            if (eventType.HasNoValue)
                return Result.Fail<IEvent>(
                    EventDeserializerErrors.UnableToFindTypeForEvent.Build(@event.Type, @event.Namespace));

            return Maybe<IEvent>.From(JsonConvert.DeserializeObject(@event.Payload, eventType.Value) as IEvent)
                .ToResult(EventDeserializerErrors.UnableToDeserializeEvent.Build());
        }
    }
}