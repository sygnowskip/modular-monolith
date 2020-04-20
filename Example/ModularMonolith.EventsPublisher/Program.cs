using System;
using System.Threading.Tasks;
using Hexure.Events;
using Hexure.EventsPublisher;
using Hexure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.Persistence;
using ModularMonolith.Registrations.Contracts.Events;

namespace ModularMonolith.EventsPublisher
{
    class Program
    {
        static async Task Main()
        {
            var batchSize = 20;
            var delay = TimeSpan.FromSeconds(10);
            var configuration = ApplicationSettingsConfigurationProvider.Get();

            await EventsPublisherFactory
                .CreatePublisher(batchSize, delay)
                .ToRabbitMqWithBaseInterface<IEvent>(configuration.GetSection("Bus").Get<EventsPublisherRabbitMqSettings>())
                .WithDbContext<MonolithDbContext>()
                .WithTransactionProvider<MonolithDbContext>()
                .WithEventsFromAssemblyOfType<RegistrationPaid>()
                .WithPersistence(services =>
                {
                    services.AddPersistence(configuration.GetConnectionString("Database"),
                        configuration.GetConnectionString("Database"));

                    services.TryAddTransient<ISystemTimeProvider, SystemTimeProvider>();
                })
                .Build()
                .RunAsync();
        }
    }
}
