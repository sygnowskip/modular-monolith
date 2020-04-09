using Hexure.Events.Namespace;
using Hexure.Results;
using Hexure.Results.Extensions;
using Newtonsoft.Json;

namespace Hexure.Events.Serialization
{
    public static class EventSerializerErrors
    {
        public static Error.ErrorType UnableToSerializeEventFromAssemblyWithoutDefinedNamespace = new Error.ErrorType(nameof(UnableToSerializeEventFromAssemblyWithoutDefinedNamespace),
             $"Unable to serialize event from assembly without defined {nameof(EventNamespace)}");

        public static Error.ErrorType UnableToSerializeNullEvent = new Error.ErrorType(nameof(UnableToSerializeNullEvent),
             $"Unable to serialize null event");
        
        public static Error.ErrorType UnableToFindTypeForEvent = new Error.ErrorType(nameof(UnableToFindTypeForEvent),
             "Unable to find type for event {0} from namespace {1}");

        public static Error.ErrorType UnableToDeserializeNullEvent = new Error.ErrorType(nameof(UnableToDeserializeNullEvent),
            $"Unable to deserialize null event");

        public static Error.ErrorType UnableToDeserializeEvent = new Error.ErrorType(nameof(UnableToDeserializeEvent),
            $"Unable to deserialize event");
    }

    public interface IEventSerializer
    {
        Result<IEvent> Deserialize(SerializedEvent @event);
        Result<SerializedEvent> Serialize<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }

    public class EventSerializer : IEventSerializer
    {
        private readonly IEventNamespaceReader _eventNamespaceReader;
        private readonly IEventTypeProvider _eventTypeProvider;

        public EventSerializer(IEventNamespaceReader eventNamespaceReader, IEventTypeProvider eventTypeProvider)
        {
            _eventNamespaceReader = eventNamespaceReader;
            _eventTypeProvider = eventTypeProvider;
        }

        public Result<IEvent> Deserialize(SerializedEvent @event)
        {
            if (@event == null)
                return Result.Fail<IEvent>(EventSerializerErrors.UnableToDeserializeNullEvent.Build());

            var eventType = _eventTypeProvider.GetType(@event.Namespace, @event.Type);
            if (eventType.HasNoValue)
                return Result.Fail<IEvent>(
                    EventSerializerErrors.UnableToFindTypeForEvent.Build(@event.Type, @event.Namespace));

            return Maybe<IEvent>.From(JsonConvert.DeserializeObject(@event.Payload, eventType.Value) as IEvent)
                .ToResult(EventSerializerErrors.UnableToDeserializeEvent.Build());
        }

        public Result<SerializedEvent> Serialize<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            if (@event == null)
                return Result.Fail<SerializedEvent>(EventSerializerErrors.UnableToSerializeNullEvent.Build());

            var @namespace = _eventNamespaceReader.GetFromAssemblyOfType<TEvent>();
            if (@namespace.HasNoValue)
                return Result.Fail<SerializedEvent>(EventSerializerErrors
                    .UnableToSerializeEventFromAssemblyWithoutDefinedNamespace.Build());

            return Result.Ok(new SerializedEvent(@namespace.Value.Name, typeof(TEvent).Name,
                JsonConvert.SerializeObject(@event)));
        }
    }
}