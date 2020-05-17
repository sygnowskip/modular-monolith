using System;
using System.Text.RegularExpressions;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace Hexure.MassTransit.Events
{
    public interface IMessageTypeProvider
    {
        Result<string> GetMessageType<TType>();
    }

    public interface IMessageTypeParser
    {
        bool IsEventNamespaceType(string messageType);
        (string Namespace, string Type) Parse(string messageType);
    }

    public class EventNamespaceMessageTypeProvider : IMessageTypeProvider, IMessageTypeParser
    {
        private readonly Regex _regex = new Regex("^event-namespace:message:(?<namespace>.*):(?<type>.*)$");
        private readonly IEventNameProvider _eventNameProvider;

        public EventNamespaceMessageTypeProvider(IEventNameProvider eventNameProvider)
        {
            _eventNameProvider = eventNameProvider;
        }

        public Result<string> GetMessageType<TType>()
        {
            return _eventNameProvider.GetEventName<TType>()
                .OnSuccess(eventName => $"event-namespace:message:{eventName}");
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