using System;
using ExternalSystem.Events.Locations;
using GreenPipes;
using Hexure.EventsPublisher;
using Hexure.MassTransit;
using Hexure.MassTransit.RabbitMq.Settings;
using Hexure.Time;
using Hexure.Workers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.Exams.Events;
using ModularMonolith.Persistence;
using ModularMonolith.Registrations.Contracts.Events;
using Newtonsoft.Json;

namespace ModularMonolith.EventsPublisher
{
    public class EventsPublisherStartup : HealthCheckEndpointStartup
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = ApplicationSettingsConfigurationProvider.Get();
            var rabbitMqSettings = configuration.GetSection("Bus").Get<PublisherRabbitMqSettings>();

            EventsPublisherBuilder
                .Create(serviceCollection)
                .WithSettings(configuration.GetSection("Settings").Get<EventsPublisherSettings>())
                .WithDbContext<MonolithDbContext>()
                .WithTransactionProvider<MonolithDbContext>()
                .WithEventsFromAssemblyOfType<RegistrationPaid>()
                .WithEventsFromAssemblyOfType<LocationAdded>()
                .WithEventsFromAssemblyOfType<ExamPlanned>()
                .WithPersistence(services =>
                {
                    services.AddPersistence(
                        configuration.GetConnectionString("Database"),
                        configuration.GetConnectionString("Database"));
                    services.TryAddTransient<ISystemTimeProvider, SystemTimeProvider>();
                })
                .ToRabbitMq(rabbitMqSettings)
                .Build();

            serviceCollection.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database"))
                .AddRabbitMQ(rabbitConnectionString: RabbitMqConnectionStringBuilder.Build(rabbitMqSettings.Host,
                    rabbitMqSettings.Username, rabbitMqSettings.Password));

        }
        
        public override void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
#if DEBUG
            var massTransitConfiguration =
                JsonConvert.SerializeObject(serviceProvider.GetRequiredService<IBusControl>().GetProbeResult());
#endif
            base.Configure(app, serviceProvider);
        }
    }
}