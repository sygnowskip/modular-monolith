using System;
using System.Collections.Concurrent;

namespace Hexure.RabbitMQ.Serialization
{
    internal static class EventNamespaceTypeMetadataCache
    {
        private static readonly ConcurrentDictionary<Type, string> _typesMessageNameCache;

        static EventNamespaceTypeMetadataCache()
        {
            _typesMessageNameCache = new ConcurrentDictionary<Type, string>();
        }

        public static string GetMessageType<TType>(IEventNameProvider eventNameProvider)
        {
            if (_typesMessageNameCache.TryGetValue(typeof(TType), out var type))
                return type;

            var typeName = CreateTypeName<TType>(eventNameProvider);
            if (_typesMessageNameCache.TryAdd(typeof(TType), typeName))
                return typeName;

            return typeName;
        }

        private static string CreateTypeName<TType>(IEventNameProvider eventNameProvider)
        {
            return $"urn:message:{eventNameProvider.GetEventName<TType>()}";
        }
    }
}