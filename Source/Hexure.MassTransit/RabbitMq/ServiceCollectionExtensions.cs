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
using Hexure.MassTransit.Inbox;
using Hexure.MassTransit.RabbitMq.Consumers;
using Hexure.MassTransit.RabbitMq.Formatters;
using Hexure.MassTransit.RabbitMq.Settings;
using Hexure.MassTransit.RabbitMq.Transactions;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.MassTransit.RabbitMq
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterRabbitMqPublisher(this IServiceCollection serviceCollection,
            PublisherRabbitMqSettings rabbitMqSettings)
        {
            RegisterRabbitMq(serviceCollection, rabbitMqSettings);
        }

        public static void RegisterRabbitMqConsumer(this IServiceCollection serviceCollection,
            ConsumerRabbitMqSettings rabbitMqSettings, ICollection<Assembly> withConsumersFromAssemblies)
        {
            serviceCollection.AddEventTypeProvider(new ConsumersEventTypeProviderBuilder(new EventNamespaceReader())
                .AddEventsFromAssemblies(withConsumersFromAssemblies)
                .Build());

            serviceCollection.AddMassTransitHostedService();

            RegisterRabbitMq(serviceCollection, rabbitMqSettings, (busConfigurator, provider) =>
                {
                    busConfigurator.UseRetry(x =>
                        x.Incremental(2, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(12)));
                    busConfigurator.UseServiceScope(provider);
                    busConfigurator.ReceiveEndpointForEachConsumer(provider, rabbitMqSettings.QueuePrefix,
                        withConsumersFromAssemblies,
                        configurator =>
                        {
                            configurator.PrefetchCount = 64;
                        });

                    busConfigurator.UseConsumeFilter(typeof(TransactionFilter<>), provider);
                    busConfigurator.UseConsumeFilter(typeof(ProcessedEventFilter<>), provider);
                },
                configurator =>
                {
                    configurator.AddConsumers(ConsumersProvider.GetConsumers(withConsumersFromAssemblies).ToArray());
                });
        }

        private static void RegisterRabbitMq(IServiceCollection serviceCollection,
            PublisherRabbitMqSettings rabbitMqSettings,
            Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext> rabbitMqBusConfiguratorAction = null,
            Action<IServiceCollectionBusConfigurator> configuratorAction = null)
        {
            serviceCollection.AddEventsMassTransitSerializers();

            serviceCollection.AddMassTransit(configurator =>
            {
                configurator.UsingRabbitMq((context, factoryConfigurator) =>
                {
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
                            context.GetRequiredService<IEventNameProvider>()));

                    factoryConfigurator.SetMessageSerializer(context
                        .GetRequiredService<EventNamespaceMessageSerializer>);
                    factoryConfigurator.AddMessageDeserializer(JsonMessageSerializer.JsonContentType,
                        context.GetRequiredService<EventNamespaceMessageDeserializer>);

                    rabbitMqBusConfiguratorAction?.Invoke(factoryConfigurator, context);
                });

                configuratorAction?.Invoke(configurator);
            });
        }
    }
}