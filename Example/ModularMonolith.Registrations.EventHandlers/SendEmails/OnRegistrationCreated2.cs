using System;
using System.Threading.Tasks;
using MassTransit;
using ModularMonolith.Registrations.Contracts.Events;
using Newtonsoft.Json;
using IMediator = MediatR.IMediator;

namespace ModularMonolith.Registrations.EventHandlers.SendEmails
{
    public class OnRegistrationCreated2 : IConsumer<RegistrationCreated>
    {
        private readonly IMediator _mediator;

        public OnRegistrationCreated2(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<RegistrationCreated> context)
        {
            throw new Exception();
            //await _mediator.Send(new StartPaymentForRegistration(context.Message.Id, new PaymentId(Guid.NewGuid())));

            Console.WriteLine($"Consumed 2 {nameof(RegistrationCreated)} with {JsonConvert.SerializeObject(context.Message)}");
        }
    }
}