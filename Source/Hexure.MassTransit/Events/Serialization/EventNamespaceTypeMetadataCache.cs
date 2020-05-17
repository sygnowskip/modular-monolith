using System;
using System.Collections.Concurrent;
using System.Linq;
using Hexure.Results;
using Hexure.Results.Extensions;
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
            var messageType = GetMessageType<TType>(messageTypeProvider);

            if (messageType.IsSuccess)
                return TypeMetadataCache<TType>.MessageTypeNames.Concat(new[]
                    {
                        messageType.Value
                    })
                    .ToArray();

            return TypeMetadataCache<TType>.MessageTypeNames;
        }

        private static Result<string> GetMessageType<TType>(IMessageTypeProvider messageTypeProvider)
        {
            if (TypesMessageNameCache.TryGetValue(typeof(TType), out var type))
                return Result.Ok(type);

            return messageTypeProvider.GetMessageType<TType>()
                .OnSuccess(messageType => { TypesMessageNameCache.TryAdd(typeof(TType), messageType); });
        }
    }
}