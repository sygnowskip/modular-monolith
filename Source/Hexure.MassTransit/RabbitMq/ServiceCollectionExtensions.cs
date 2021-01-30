using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GreenPipes;
using Hexure.Events;
using Hexure.Events.Namespace;
using Hexure.MassTransit.Events;
using Hexure.MassTransit.Events.Builders;
using Hexure.MassTransit.Events.Serialization;
using Hexure.MassTransit.RabbitMq.Consumers;
using Hexure.MassTransit.RabbitMq.Formatters;
using Hexure.MassTransit.RabbitMq.Settings;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.MassTransit.RabbitMq
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterRabbitMqPublisher(this IServiceCollection serviceCollection, PublisherRabbitMqSettings rabbitMqSettings)
        {
            RegisterRabbitMq(serviceCollection, rabbitMqSettings);
        }

        public static void RegisterRabbitMqConsumer(this IServiceCollection serviceCollection, ConsumerRabbitMqSettings rabbitMqSettings, ICollection<Assembly> withConsumersFromAssemblies)
        {
            serviceCollection.AddEventTypeProvider(new ConsumersEventTypeProviderBuilder(new EventNamespaceReader())
                .AddEventsFromAssemblies(withConsumersFromAssemblies)
                .Build());

            serviceCollection.AddMassTransitHostedService();

            RegisterRabbitMq(serviceCollection, rabbitMqSettings, (busConfigurator, provider) =>
                {
                    busConfigurator.ReceiveEndpointForEachConsumer(provider, rabbitMqSettings.QueuePrefix, withConsumersFromAssemblies,
                        configurator =>
                        {
                            configurator.PrefetchCount = 25;
                            configurator.UseMessageRetry(x =>
                                x.Incremental(2, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(12)));
                        });
                },
                configurator =>
                {
                    configurator.AddConsumers(ConsumersProvider.GetConsumers(withConsumersFromAssemblies).ToArray());
                });
        }

        private static void RegisterRabbitMq(IServiceCollection serviceCollection,
            PublisherRabbitMqSettings rabbitMqSettings,
            Action<IRabbitMqBusFactoryConfigurator, IServiceProvider> rabbitMqBusConfiguratorAction = null,
            Action<IServiceCollectionBusConfigurator> configuratorAction = null)
        {
            serviceCollection.AddEventsMassTransitSerializers();

            serviceCollection.AddMassTransit(configurator =>
            {
                //TODO: Migrate to CreateUsingRabbitMq (MassTrasit@7 change)
                //https://masstransit-project.com/getting-started/upgrade-v6.html#version-7
                configurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(factoryConfigurator =>
                {
                    factoryConfigurator.UseHealthCheck(provider);
                    
                    factoryConfigurator.Host(rabbitMqSettings.Host, hostConfigurator =>
                    {
                        hostConfigurator.Username(rabbitMqSettings.Username);
                        hostConfigurator.Password(rabbitMqSettings.Password);
                        hostConfigurator.ConfigureBatchPublish(publishConfigurator =>
                        {
                            publishConfigurator.Enabled = true;
                            publishConfigurator.Timeout = TimeSpan.FromMilliseconds(50);
                        });
                    });

                    factoryConfigurator.MessageTopology.SetEntityNameFormatter(
                        new EventNamespaceNameFormatter(
                            factoryConfigurator.MessageTopology.EntityNameFormatter,
                            provider.GetRequiredService<IEventNameProvider>()));
                    
                    factoryConfigurator.SetMessageSerializer(provider.GetRequiredService<EventNamespaceMessageSerializer>);
                    factoryConfigurator.AddMessageDeserializer(JsonMessageSerializer.JsonContentType,
                        provider.GetRequiredService<EventNamespaceMessageDeserializer>);

                    rabbitMqBusConfiguratorAction?.Invoke(factoryConfigurator, provider);
                }));

                configuratorAction?.Invoke(configurator);
            });
        }
    }
}