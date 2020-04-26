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
        private readonly IMessageTypeProvider _messageTypeProvider;

        public EventNamespaceTypeMetadataService(IMessageTypeProvider messageTypeProvider)
        {
            _messageTypeProvider = messageTypeProvider;
        }


        public string[] GetMessageTypes<TType>()
        {
            return TypeMetadataCache<TType>.MessageTypeNames.Concat(new[]
                {
                    EventNamespaceTypeMetadataCache.GetMessageType<TType>(_messageTypeProvider)
                })
                .ToArray();
        }


    }
}