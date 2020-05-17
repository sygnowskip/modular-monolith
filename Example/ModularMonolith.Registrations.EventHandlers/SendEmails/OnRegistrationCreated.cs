using System;
using System.Threading.Tasks;
using MassTransit;
using ModularMonolith.Payments.Language;
using ModularMonolith.Registrations.Commands;
using ModularMonolith.Registrations.Contracts.Events;
using Newtonsoft.Json;
using IMediator = MediatR.IMediator;

namespace ModularMonolith.Registrations.EventHandlers.SendEmails
{
    public class OnRegistrationCreated : IConsumer<RegistrationCreated>
    {
        private readonly IMediator _mediator;

        public OnRegistrationCreated(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<RegistrationCreated> context)
        {
            await _mediator.Send(new StartPaymentForRegistration(context.Message.Id, new PaymentId(Guid.NewGuid())));

            Console.WriteLine($"Consumed {nameof(RegistrationCreated)} with {JsonConvert.SerializeObject(context.Message)}");
        }
    }
}