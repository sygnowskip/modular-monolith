using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Hexure.EventsConsumer;
using Hexure.MassTransit.RabbitMq.Settings;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.Registrations.EventHandlers.SendEmails;

namespace ModularMonolith.EventsConsumer
{
    public class Program
    {
        public static async Task Main()
        {
            var configuration = ApplicationSettingsConfigurationProvider.Get();

            await EventsConsumerFactory
                .Create()
                .WithHandlersFromAssemblyOfType<OnRegistrationPaid>()
                .WithDomain(services =>
                {
                    services.AddRegistrations();
                    services.AddPayments();
                    services.AddPersistence(configuration.GetConnectionString("Database"),
                        configuration.GetConnectionString("Database"));
                })
                .ToRabbitMq(configuration.GetSection("Bus").Get<ConsumerRabbitMqSettings>())
                .Build()
                .RunAsync();
        }
    }
}
