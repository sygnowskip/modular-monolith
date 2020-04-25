using System;
using Hexure.Events.Namespace;

namespace Hexure.RabbitMQ
{
    public interface IEventNameProvider
    {
        string GetEventName<T>();
    }

    public class EventNamespaceEventNameProvider : IEventNameProvider
    {
        private readonly IEventNamespaceReader _eventNamespaceReader;

        public EventNamespaceEventNameProvider(IEventNamespaceReader eventNamespaceReader)
        {
            _eventNamespaceReader = eventNamespaceReader;
        }

        public string GetEventName<T>()
        {
            var ns = _eventNamespaceReader.GetFromAssemblyOfType<T>();
            if (ns.HasNoValue)
                throw new InvalidOperationException($"Unable to create name for {typeof(T).FullName} message due to missing {nameof(EventNamespace)} attribute");

            return $"{ns.Value.Name}:{typeof(T).Name}";
        }
    }
}