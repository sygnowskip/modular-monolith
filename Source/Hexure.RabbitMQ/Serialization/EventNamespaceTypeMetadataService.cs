using System.Linq;
using MassTransit.Metadata;

namespace Hexure.RabbitMQ.Serialization
{
    public interface IEventNamespaceTypeMetadataService
    {
        string[] GetMessageTypes<TType>();
    }

    public class EventNamespaceTypeMetadataService : IEventNamespaceTypeMetadataService
    {
        private readonly IEventNameProvider _eventNameProvider;

        public EventNamespaceTypeMetadataService(IEventNameProvider eventNameProvider)
        {
            _eventNameProvider = eventNameProvider;
        }


        public string[] GetMessageTypes<TType>()
        {
            return TypeMetadataCache<TType>.MessageTypeNames.Concat(new[]
                {
                    EventNamespaceTypeMetadataCache.GetMessageType<TType>(_eventNameProvider)
                })
                .ToArray();
        }


    }
}