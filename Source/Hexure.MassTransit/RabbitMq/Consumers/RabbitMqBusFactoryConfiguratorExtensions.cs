using System;
using System.Collections.Generic;
using System.Reflection;
using MassTransit;
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
            foreach (var consumer in ConsumersProvider.GetConsumers(fromAssemblies))
            {
                configurator.ReceiveEndpoint($"{queuePrefix}:{consumer.FullName}", endpointConfigurator =>
                {
                    endpointConfigurator.Consumer(consumer, type => provider.CreateScope().ServiceProvider.GetRequiredService(type));
                    endpointConfiguration?.Invoke(endpointConfigurator);
                });
            }
        }
    }
}