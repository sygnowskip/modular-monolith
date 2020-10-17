using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MassTransit.Metadata;

namespace Hexure.MassTransit.RabbitMq.Consumers
{
    public static class ConsumersProvider
    {
        public static ICollection<Type> GetConsumers(ICollection<Assembly> fromAssemblies)
        {
            return fromAssemblies.SelectMany(a => a.GetTypes())
                .Where(TypeMetadataCache.IsConsumerOrDefinition)
                .Select(type => type)
                .ToList();
        }
    }
}