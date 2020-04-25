using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GreenPipes;
using Hexure.Events.Namespace;
using Hexure.RabbitMQ.Serialization;
using Hexure.RabbitMQ.Settings;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.Topology;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hexure.RabbitMQ
{
    public static class RabbitConnector
    {
        public static void RegisterRabbitMqPublisher(IServiceCollection serviceCollection, PublisherRabbitMqSettings rabbitMqSettings)
        {
            serviceCollection.TryAddTransient<IEventNamespaceReader, EventNamespaceReader>();
            serviceCollection.TryAddTransient<IEventNameProvider, EventNamespaceEventNameProvider>();
            serviceCollection.TryAddTransient<IEventNamespaceTypeMetadataService, EventNamespaceTypeMetadataService>();
            serviceCollection.TryAddTransient<EventNamespaceMessageSerializer>();

            RegisterRabbitMq(serviceCollection, rabbitMqSettings);
        }

        public static void RegisterRabbitMqConsumer(IServiceCollection serviceCollection, ConsumerRabbitMqSettings rabbitMqSettings, IEnumerable<Assembly> withConsumersFromAssemblies)
        {
            RegisterRabbitMq(serviceCollection, rabbitMqSettings, (busConfigurator, provider) =>
                {
                    busConfigurator.ReceiveEndpoint(rabbitMqSettings.Queue, endpointConfigurator =>
                    {
                        endpointConfigurator.PrefetchCount = 10;
                        endpointConfigurator.UseMessageRetry(x =>
                            x.Incremental(2, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(12)));
                        endpointConfigurator.ConfigureConsumers(provider);
                    });
                },
                configurator => configurator.AddConsumers(withConsumersFromAssemblies.ToArray()));
        }

        private static void RegisterRabbitMq(IServiceCollection serviceCollection,
            PublisherRabbitMqSettings rabbitMqSettings,
            Action<IRabbitMqBusFactoryConfigurator, IServiceProvider> rabbitMqBusConfiguratorAction = null,
            Action<IServiceCollectionConfigurator> configuratorAction = null)
        {
            serviceCollection.AddMassTransit(configurator =>
            {
                configurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(factoryConfigurator =>
                {
                    factoryConfigurator.Host(rabbitMqSettings.Host, hostConfigurator =>
                    {
                        hostConfigurator.Username(rabbitMqSettings.Username);
                        hostConfigurator.Password(rabbitMqSettings.Password);
                    });

                    factoryConfigurator.MessageTopology.SetEntityNameFormatter(
                        new EventNamespaceNameFormatter(
                            factoryConfigurator.MessageTopology.EntityNameFormatter,
                            provider.GetRequiredService<IEventNameProvider>()));
                    
                    factoryConfigurator.SetMessageSerializer(provider.GetRequiredService<EventNamespaceMessageSerializer>);

                    rabbitMqBusConfiguratorAction?.Invoke(factoryConfigurator, provider);
                }));

                configuratorAction?.Invoke(configurator);
            });
        }
    }
}