using System;
using System.Collections.Generic;
using System.Reflection;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Hexure.MassTransit.RabbitMq.Consumers
{
    public static class RabbitMqBusFactoryConfiguratorExtensions
    {
        public static void ReceiveEndpointForEachConsumer(this IRabbitMqBusFactoryConfigurator configurator,
            IBusRegistrationContext provider, string queuePrefix, ICollection<Assembly> fromAssemblies,
            Action<IRabbitMqReceiveEndpointConfigurator> endpointConfiguration = null)
        {
            foreach (var consumer in ConsumersProvider.GetConsumers(fromAssemblies))
            {
                configurator.ReceiveEndpoint($"{queuePrefix}:{consumer.FullName}", endpointConfigurator =>
                {
                    endpointConfigurator.ConfigureConsumer(provider, consumer);
                    endpointConfiguration?.Invoke(endpointConfigurator);
                });
            }
        }
    }
}