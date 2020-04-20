using System;
using Hexure.Events.Namespace;
using MassTransit.Topology;

namespace Hexure.EventsPublisher
{
    public class EventNamespaceNameFormatter<TDefaultEventType> : IEntityNameFormatter
    {
        private readonly IEntityNameFormatter _defaultEntityNameFormatter;
        private readonly IEventNamespaceReader _eventNamespaceReader;

        public EventNamespaceNameFormatter(IEntityNameFormatter defaultEntityNameFormatter, IEventNamespaceReader eventNamespaceReader)
        {
            _eventNamespaceReader = eventNamespaceReader;
            _defaultEntityNameFormatter = defaultEntityNameFormatter;
        }

        public string FormatEntityName<T>()
        {
            if (typeof(T) == typeof(TDefaultEventType))
                return _defaultEntityNameFormatter.FormatEntityName<T>();

            var ns = _eventNamespaceReader.GetFromAssemblyOfType<T>();
            if (ns.HasNoValue)
                throw new InvalidOperationException($"Unable to publish {typeof(T).FullName} message due to missing {nameof(EventNamespace)} attribute");

            return $"{ns.Value.Name}:{typeof(T).Name}";
        }
    }
}