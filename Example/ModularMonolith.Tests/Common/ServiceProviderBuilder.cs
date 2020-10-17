using System;
using Hexure.Events;
using Hexure.Events.Namespace;
using Hexure.MassTransit.Events.Builders;
using Hexure.MassTransit.RabbitMq;
using Hexure.MassTransit.RabbitMq.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Configuration;

namespace ModularMonolith.Tests.Common
{
    public class ServiceProviderBuilder
    {
        public static IServiceProvider Build()
        {
            var applicationSettings = ApplicationSettingsConfigurationProvider.Get();
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            serviceCollection.AddDomainEvents();
            serviceCollection.AddEventTypeProvider(new ConsumersEventTypeProviderBuilder(new EventNamespaceReader()).Build());
            serviceCollection.RegisterRabbitMqPublisher(applicationSettings.GetSection("Bus")
                .Get<ConsumerRabbitMqSettings>());

            return serviceCollection.BuildServiceProvider();
        }
    }
}