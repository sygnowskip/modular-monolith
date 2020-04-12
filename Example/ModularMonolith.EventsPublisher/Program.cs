using System;
using System.Threading.Tasks;
using Hexure.EventsPublisher;
using Microsoft.Extensions.Configuration;
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
                .WithDbContext<MonolithDbContext>()
                .WithTransactionProvider<MonolithDbContext>()
                .WithEventsFromAssemblyOfType<RegistrationPaid>()
                .WithDomain(services =>
                {
                    services.AddRegistrations();
                    services.AddPayments();
                    services.AddPersistence(configuration.GetConnectionString("Database"),
                        configuration.GetConnectionString("Database"));
                })
                .Build()
                .RunAsync();
        }
    }
}
