using System;
using System.Collections.Generic;
using System.Reflection;
using Hexure.Events.Namespace;

namespace Hexure.Events.Serialization
{
    public class EventTypeProviderBuilder
    {
        private readonly IEventNamespaceReader _eventNamespaceReader;

        private readonly Dictionary<string, Assembly> _namespaces = new Dictionary<string, Assembly>();

        public EventTypeProviderBuilder(IEventNamespaceReader eventNamespaceReader)
        {
            _eventNamespaceReader = eventNamespaceReader;
        }

        public void AddEventsFromAssemblyOfType(Type eventType)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
                throw new InvalidOperationException(
                    $"Unable to register events without {nameof(IEvent)} interface");

            var ns = _eventNamespaceReader.GetFromAssemblyOfType(eventType);
            if (ns.HasNoValue)
                throw new InvalidOperationException(
                    "Unable to register events from assembly without [EventNamespace] attribute");

            if (_namespaces.ContainsKey(ns.Value.Name))
            {
                if (_namespaces[ns.Value.Name] == eventType.Assembly)
                    return;

                throw new InvalidOperationException(
                    "Unable to register events from assemblies with duplicated values of [EventNamespace] attribute");
            }

            _namespaces.Add(ns.Value.Name, eventType.Assembly);
        }

        public void AddEventsFromAssemblyOfType<TType>()
            where TType : IEvent
        {
            var ns = _eventNamespaceReader.GetFromAssemblyOfType<TType>();
            if (ns.HasNoValue)
                throw new InvalidOperationException(
                    "Unable to publish events from assembly without [EventNamespace] attribute");

            if (_namespaces.ContainsKey(ns.Value.Name))
                throw new InvalidOperationException(
                    "Unable to publish events from assemblies with duplicated values of [EventNamespace] attribute");

            _namespaces.Add(ns.Value.Name, typeof(TType).Assembly);
        }

        public IEventTypeProvider Build() => new EventTypeProvider(_namespaces);
    }
}