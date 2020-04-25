﻿using System;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using MassTransit;
using MassTransit.Serialization;
using Newtonsoft.Json;

namespace Hexure.RabbitMQ.Serialization
{
    public class EventNamespaceMessageSerializer : IMessageSerializer
    {
        static readonly Lazy<JsonSerializer> Serializer;
        static readonly Lazy<Encoding> Encoding;

        static EventNamespaceMessageSerializer()
        {
            Encoding = new Lazy<Encoding>(() => new UTF8Encoding(false, true), LazyThreadSafetyMode.PublicationOnly);
            Serializer = new Lazy<JsonSerializer>(() => JsonSerializer.Create(JsonMessageSerializer.SerializerSettings));
        }

        private readonly IEventNamespaceTypeMetadataService _eventNamespaceTypeMetadataService;

        public EventNamespaceMessageSerializer(IEventNamespaceTypeMetadataService eventNamespaceTypeMetadataService)
        {
            _eventNamespaceTypeMetadataService = eventNamespaceTypeMetadataService;
        }

        public void Serialize<T>(Stream stream, SendContext<T> context) where T : class
        {
            try
            {
                context.ContentType = ContentType;

                var envelope = new JsonMessageEnvelope(context, context.Message, _eventNamespaceTypeMetadataService.GetMessageTypes<T>());

                using (var writer = new StreamWriter(stream, Encoding.Value, 1024, true))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.Indented;

                    Serializer.Value.Serialize(jsonWriter, envelope, typeof(MessageEnvelope));

                    jsonWriter.Flush();
                    writer.Flush();
                }
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to serialize message", ex);
            }

        }

        public ContentType ContentType => JsonMessageSerializer.JsonContentType;
    }
}