using Hexure.EventsConsumer;
using Hexure.MassTransit;
using Hexure.MassTransit.Inbox;
using Hexure.MassTransit.RabbitMq.Settings;
using Hexure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.Persistence;
using ModularMonolith.ReadModels.EventHandlers.UpdateLocations;

namespace ModularMonolith.EventsConsumer
{
    public class EventsConsumerStartup : HealthCheckEndpointStartup
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = ApplicationSettingsConfigurationProvider.Get();
            var rabbitMqSettings = configuration.GetSection("Bus").Get<ConsumerRabbitMqSettings>();

            EventsConsumerBuilder
                .Create(serviceCollection)
                .WithHandlersFromAssemblyOfType<OnLocationAdded>()
                .WithServices(services =>
                {
                    services.AddRegistrations();
                    services.AddPayments();
                    services.AddPersistence(
                        configuration.GetConnectionString("Database"),
                        configuration.GetConnectionString("Database"));
                    services.AddInbox<MonolithDbContext>();
                    services.AddMemoryCache();
                })
                .ToRabbitMq(rabbitMqSettings)
                .Build();

            serviceCollection.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database"))
                .AddRabbitMQ(rabbitConnectionString: RabbitMqConnectionStringBuilder.Build(rabbitMqSettings.Host,
                    rabbitMqSettings.Username, rabbitMqSettings.Password));
        }
    }
}