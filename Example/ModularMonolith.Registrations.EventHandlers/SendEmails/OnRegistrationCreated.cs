using System;
using System.Threading.Tasks;
using MassTransit;
using ModularMonolith.Registrations.Contracts.Events;
using ModularMonolith.Registrations.Contracts.Events.Different;
using Newtonsoft.Json;

namespace ModularMonolith.Registrations.EventHandlers.SendEmails
{
    public class OnRegistrationCreated : IConsumer<RegistrationCreated>
    {
        public Task Consume(ConsumeContext<RegistrationCreated> context)
        {
            Console.WriteLine($"Consumed {nameof(RegistrationCreated)} with {JsonConvert.SerializeObject(context.Message)}");
            return Task.CompletedTask;
        }
    }
}