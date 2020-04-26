using System;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using GreenPipes;
using Hexure.Events.Serialization;
using MassTransit;
using MassTransit.Serialization;
using MassTransit.Util;
using Newtonsoft.Json;

namespace Hexure.RabbitMQ.Serialization
{
    public class EventNamespaceMessageDeserializer : IMessageDeserializer
    {
        private readonly JsonSerializer _deserializer;
        private readonly IMessageTypeParser _messageTypeParser;
        private readonly IEventTypeProvider _eventTypeProvider;

        public EventNamespaceMessageDeserializer(JsonSerializer deserializer, IEventTypeProvider eventTypeProvider, IMessageTypeParser messageTypeParser)
        {
            _deserializer = deserializer;
            _eventTypeProvider = eventTypeProvider;
            _messageTypeParser = messageTypeParser;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("json").Add("contentType", JsonMessageSerializer.JsonContentType.MediaType);
        }

        public ConsumeContext Deserialize(ReceiveContext receiveContext)
        {
            try
            {
                var messageEncoding = GetMessageEncoding(receiveContext);

                using (var body = receiveContext.GetBodyStream())
                using (var reader = new StreamReader(body, messageEncoding, false, 1024, true))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var envelope = _deserializer.Deserialize<EventNamespaceMessageEnvelope>(jsonReader);
                    envelope.ApplyEventNamespaceBinding(_messageTypeParser, _eventTypeProvider);
                    return new JsonConsumeContext(_deserializer, receiveContext, envelope);
                }
            }
            catch (JsonSerializationException ex)
            {
                throw new SerializationException("A JSON serialization exception occurred while deserializing the message envelope", ex);
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("An exception occurred while deserializing the message envelope", ex);
            }
        }

        public ContentType ContentType => JsonMessageSerializer.JsonContentType;

        static Encoding GetMessageEncoding(ReceiveContext receiveContext)
        {
            var contentEncoding = receiveContext.TransportHeaders.Get("Content-Encoding", default(string));

            return string.IsNullOrWhiteSpace(contentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(contentEncoding);
        }
    }
}