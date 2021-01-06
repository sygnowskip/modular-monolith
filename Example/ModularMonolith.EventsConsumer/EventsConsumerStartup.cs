using Hexure.EventsConsumer;
using Hexure.MassTransit.RabbitMq.Settings;
using Hexure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.ReadModels.EventHandlers.UpdateLocations;

namespace ModularMonolith.EventsConsumer
{
    public class EventsConsumerStartup : HealthCheckEndpointStartup
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = ApplicationSettingsConfigurationProvider.Get();

            EventsConsumerBuilder
                .Create(serviceCollection)
                .WithHandlersFromAssemblyOfType<OnLocationAdded>()
                .WithDomain(services =>
                {
                    services.AddRegistrations();
                    services.AddPayments();
                    services.AddPersistence(configuration.GetConnectionString("Database"));
                })
                .ToRabbitMq(configuration.GetSection("Bus").Get<ConsumerRabbitMqSettings>())
                .Build();

            serviceCollection.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database"));
        }
    }
}