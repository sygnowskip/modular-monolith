using System;
using System.Text.RegularExpressions;
using Hexure.Events.Namespace;

namespace Hexure.RabbitMQ
{
    public interface IEventNameProvider
    {
        string GetEventName<T>();
    }

    public interface IMessageTypeProvider
    {
        string GetEventMessageType<TType>();
    }

    public interface IMessageTypeParser
    {
        bool IsEventNamespaceType(string messageType);
        (string Namespace, string Type) Parse(string messageType);
    }

    public class EventNamespaceNameProvider : IEventNameProvider, IMessageTypeProvider, IMessageTypeParser
    {
        private readonly IEventNamespaceReader _eventNamespaceReader;
        private readonly Regex _regex = new Regex("^event-namespace:message:(?<namespace>.*):(?<type>.*)$");

        public EventNamespaceNameProvider(IEventNamespaceReader eventNamespaceReader)
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

        public string GetEventMessageType<TType>()
        {
            return $"event-namespace:message:{GetEventName<TType>()}";
        }

        public bool IsEventNamespaceType(string messageType)
        {
            return _regex.IsMatch(messageType);
        }

        public (string Namespace, string Type) Parse(string messageType)
        {
            var parsed = _regex.Match(messageType);
            if (!parsed.Success)
                throw new InvalidOperationException($"Unable to get namespace and type from {messageType} due to invalid format");

            return (parsed.Groups["namespace"].Value, parsed.Groups["type"].Value);
        }
    }
}