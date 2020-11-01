/*using System;
using System.Threading.Tasks;
using MassTransit;
using ModularMonolith.Registrations.Contracts.Events;
using Newtonsoft.Json;
using IMediator = MediatR.IMediator;

namespace ModularMonolith.Registrations.EventHandlers.SendEmails
{
    public class OnRegistrationCreated : IConsumer<RegistrationCreated>, IConsumer<RegistrationPaid>
    {
        private readonly IMediator _mediator;

        public OnRegistrationCreated(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<RegistrationCreated> context)
        {
            //await _mediator.Send(new StartPaymentForRegistration(context.Message.Id, new PaymentId(Guid.NewGuid())));

            Console.WriteLine($"Consumed {nameof(RegistrationCreated)} with {JsonConvert.SerializeObject(context.Message)}");
        }

        public async Task Consume(ConsumeContext<RegistrationPaid> context)
        {
            Console.WriteLine($"Consumed {nameof(RegistrationPaid)} with {JsonConvert.SerializeObject(context.Message)}");
        }
    }
}*/