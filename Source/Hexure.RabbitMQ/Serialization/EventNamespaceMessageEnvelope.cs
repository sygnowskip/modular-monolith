using System;
using System.Collections.Generic;
using System.Linq;
using Hexure.Events.Serialization;
using MassTransit;
using MassTransit.Serialization;

namespace Hexure.RabbitMQ.Serialization
{
    public class EventNamespaceMessageEnvelope : MessageEnvelope
    {
        public string MessageId { get; set; }
        public string RequestId { get; set; }
        public string CorrelationId { get; set; }
        public string ConversationId { get; set; }
        public string InitiatorId { get; set; }
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string ResponseAddress { get; set; }
        public string FaultAddress { get; set; }
        public string[] MessageType { get; set; }
        public object Message { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public DateTime? SentTime { get; set; }
        public IDictionary<string, object> Headers { get; set; }
        public HostInfo Host { get; set; }

        public void ApplyEventNamespaceBinding(IMessageTypeParser messageTypeParser, IEventTypeProvider eventTypeProvider)
        {
            var eventNamespaceMessageType = MessageType.FirstOrDefault(messageTypeParser.IsEventNamespaceType);
            if (string.IsNullOrWhiteSpace(eventNamespaceMessageType))
                return;

            var parsed = messageTypeParser.Parse(eventNamespaceMessageType);
            var messageType = eventTypeProvider.GetType(parsed.Namespace, parsed.Type);
            if (messageType.HasNoValue)
                return;

            var messageTypes = MessageType.Where(mt => mt != eventNamespaceMessageType).ToList();
            messageTypes.Add(MessageUrn.ForType(messageType.Value).ToString());
            MessageType = messageTypes.ToArray();
        }
    }
}