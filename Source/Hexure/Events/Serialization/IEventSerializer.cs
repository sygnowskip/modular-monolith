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
    }

    public interface IEventSerializer
    {
        Result<SerializedEvent> Serialize(IEvent @event);
    }

    public class EventSerializer : IEventSerializer
    {
        private readonly IEventNamespaceReader _eventNamespaceReader;

        public EventSerializer(IEventNamespaceReader eventNamespaceReader)
        {
            _eventNamespaceReader = eventNamespaceReader;
        }

        public Result<SerializedEvent> Serialize(IEvent @event)
        {
            if (@event == null)
                return Result.Fail<SerializedEvent>(EventSerializerErrors.UnableToSerializeNullEvent.Build());

            var eventType = @event.GetType();
            var @namespace = _eventNamespaceReader.GetFromAssemblyOfType(eventType);
            if (@namespace.HasNoValue)
                return Result.Fail<SerializedEvent>(EventSerializerErrors
                    .UnableToSerializeEventFromAssemblyWithoutDefinedNamespace.Build());

            return Result.Ok(new SerializedEvent(@namespace.Value.Name, eventType.Name,
                JsonConvert.SerializeObject(@event)));
        }
    }
}