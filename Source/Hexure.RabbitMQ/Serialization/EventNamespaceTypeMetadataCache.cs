using System;
using System.Collections.Concurrent;
using Hexure.Events.Serialization;

namespace Hexure.RabbitMQ.Serialization
{
    internal static class EventNamespaceTypeMetadataCache
    {
        private static readonly ConcurrentDictionary<Type, string> TypesMessageNameCache;

        static EventNamespaceTypeMetadataCache()
        {
            TypesMessageNameCache = new ConcurrentDictionary<Type, string>();
        }

        public static string GetMessageType<TType>(IMessageTypeProvider messageTypeProvider)
        {
            if (TypesMessageNameCache.TryGetValue(typeof(TType), out var type))
                return type;

            var typeName = messageTypeProvider.GetEventMessageType<TType>();
            if (TypesMessageNameCache.TryAdd(typeof(TType), typeName))
                return typeName;

            return typeName;
        }
    }
}