using System.Threading.Tasks;
using Hexure.EventsPublisher;
using Hexure.MassTransit.RabbitMq.Settings;
using Hexure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.Persistence;
using ModularMonolith.Registrations.Contracts.Events;

namespace ModularMonolith.EventsPublisher
{
    public static class Program
    {
        //https://hub.docker.com/r/willfarrell/autoheal/
        //+ DOCKER HealthCheck
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(servicesCollection =>
                {
                    var configuration = ApplicationSettingsConfigurationProvider.Get();

                    EventsPublisherBuilder
                        .Create(servicesCollection)
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
                });
    }
}