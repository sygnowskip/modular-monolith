using MassTransit.Topology;

namespace Hexure.RabbitMQ
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

            return _eventNameProvider.GetEventName<T>();
        }
    }
}