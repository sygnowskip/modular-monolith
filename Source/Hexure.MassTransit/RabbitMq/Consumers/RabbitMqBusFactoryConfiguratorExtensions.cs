using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MassTransit;
using MassTransit.Metadata;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.MassTransit.RabbitMq.Consumers
{
    public static class RabbitMqBusFactoryConfiguratorExtensions
    {
        public static void ReceiveEndpointForEachConsumer(this IRabbitMqBusFactoryConfigurator configurator,
            IServiceProvider provider, string queuePrefix, ICollection<Assembly> fromAssemblies,
            Action<IRabbitMqReceiveEndpointConfigurator> endpointConfiguration = null)
        {
            for (var i = 0; i < 100; i++)
            {
                foreach (var consumer in GetConsumers(fromAssemblies))
                {
                    configurator.ReceiveEndpoint($"{queuePrefix}:{consumer.FullName}:{i}", endpointConfigurator =>
                    {
                        endpointConfigurator.Consumer(consumer, provider.GetRequiredService);
                        endpointConfiguration?.Invoke(endpointConfigurator);
                    });
                }
            }
            
        }

        private static ICollection<Type> GetConsumers(ICollection<Assembly> fromAssemblies)
        {
            return fromAssemblies.SelectMany(a => a.GetTypes())
                .Where(TypeMetadataCache.IsConsumerOrDefinition)
                .Select(type => type)
                .ToList();
        }
    }
}