/*using System;
using System.Threading.Tasks;
using MassTransit;
using ModularMonolith.Registrations.Contracts.Events;
using Newtonsoft.Json;

namespace ModularMonolith.Registrations.EventHandlers.SendEmails
{
    public class OnRegistrationPaid : IConsumer<RegistrationPaid>
    {
        public Task Consume(ConsumeContext<RegistrationPaid> context)
        {
            Console.WriteLine($"Consumed {nameof(RegistrationPaid)} with {JsonConvert.SerializeObject(context.Message)}");
            return Task.CompletedTask;
        }
    }
}*/