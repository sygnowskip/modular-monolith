using Hexure.EventsPublisher;
using Hexure.MassTransit.RabbitMq.Settings;
using Hexure.Time;
using Hexure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.Persistence;
using ModularMonolith.Registrations.Contracts.Events;

namespace ModularMonolith.EventsPublisher
{
    public class EventsPublisherStartup : HealthCheckEndpointStartup
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = ApplicationSettingsConfigurationProvider.Get();

            EventsPublisherBuilder
                .Create(serviceCollection)
                .WithSettings(configuration.GetSection("Settings").Get<EventsPublisherSettings>())
                .WithDbContext<MonolithDbContext>()
                .WithTransactionProvider<MonolithDbContext>()
                .WithEventsFromAssemblyOfType<RegistrationPaid>()
                .WithPersistence(services =>
                {
                    services.AddPersistence(configuration.GetConnectionString("Database"));
                    services.TryAddTransient<ISystemTimeProvider, SystemTimeProvider>();
                })
                .ToRabbitMq(configuration.GetSection("Bus").Get<PublisherRabbitMqSettings>())
                .Build();
            
            serviceCollection.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database"));
        }
    }
}