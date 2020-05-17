using System;
using Hexure.MassTransit.Events;
using MassTransit.Topology;
using Newtonsoft.Json;

namespace Hexure.MassTransit.RabbitMq.Formatters
{
    public class EventNamespaceNameFormatter : IEntityNameFormatter
    {
        private readonly IEntityNameFormatter _defaultEntityNameFormatter;
        private readonly IEventNameProvider _eventNameProvider;

        public EventNamespaceNameFormatter(IEntityNameFormatter defaultEntityNameFormatter, IEventNameProvider eventNameProvider)
        {
            _defaultEntityNameFormatter = defaultEntityNameFormatter;
            _eventNameProvider = eventNameProvider;
        }

        public string FormatEntityName<T>()
        {
            if (typeof(T).IsInterface)
                return _defaultEntityNameFormatter.FormatEntityName<T>();

            var eventName = _eventNameProvider.GetEventName<T>();

            return eventName.IsSuccess
                ? eventName.Value
                : throw new InvalidOperationException(JsonConvert.SerializeObject(eventName.Error));
        }
    }
}