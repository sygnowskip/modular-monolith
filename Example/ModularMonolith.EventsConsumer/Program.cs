using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Hexure.EventsConsumer;
using Hexure.RabbitMQ.Settings;
using ModularMonolith.Configuration;
using ModularMonolith.Dependencies;
using ModularMonolith.Registrations.EventHandlers.SendEmails;

namespace ModularMonolith.EventsConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = ApplicationSettingsConfigurationProvider.Get();

            EventsConsumerFactory
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
                .Run();
        }
    }
}
