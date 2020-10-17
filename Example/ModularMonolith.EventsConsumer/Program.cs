using Microsoft.Extensions.Configuration;
using Hexure.EventsConsumer;
using Hexure.MassTransit.RabbitMq.Settings;
using Microsoft.Extensions.Hosting;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.ReadModels.EventHandlers.UpdateLocations;

namespace ModularMonolith.EventsConsumer
{
    public static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(servicesCollection =>
                {
                    var configuration = ApplicationSettingsConfigurationProvider.Get();

                    EventsConsumerBuilder
                        .Create(servicesCollection)
                        //.WithHandlersFromAssemblyOfType<OnRegistrationPaid>()
                        .WithHandlersFromAssemblyOfType<OnLocationAdded>()
                        .WithDomain(services =>
                        {
                            services.AddRegistrations();
                            services.AddPayments();
                            services.AddPersistence(configuration.GetConnectionString("Database"));
                        })
                        .ToRabbitMq(configuration.GetSection("Bus").Get<ConsumerRabbitMqSettings>())
                        .Build();
                });

    }
}
