using System;
using System.Collections.Concurrent;
using System.Linq;
using MassTransit.Metadata;

namespace Hexure.MassTransit.Events.Serialization
{
    internal static class EventNamespaceTypeMetadataCache
    {
        private static readonly ConcurrentDictionary<Type, string> TypesMessageNameCache;

        static EventNamespaceTypeMetadataCache()
        {
            TypesMessageNameCache = new ConcurrentDictionary<Type, string>();
        }

        public static string[] GetMessageTypes<TType>(IMessageTypeProvider messageTypeProvider)
        {
            return TypeMetadataCache<TType>.MessageTypeNames.Concat(new[]
                {
                    GetMessageType<TType>(messageTypeProvider)
                })
                .ToArray();
        }

        private static string GetMessageType<TType>(IMessageTypeProvider messageTypeProvider)
        {
            if (TypesMessageNameCache.TryGetValue(typeof(TType), out var type))
                return type;

            var typeName = messageTypeProvider.GetMessageType<TType>();
            if (TypesMessageNameCache.TryAdd(typeof(TType), typeName))
                return typeName;

            return typeName;
        }
    }
}