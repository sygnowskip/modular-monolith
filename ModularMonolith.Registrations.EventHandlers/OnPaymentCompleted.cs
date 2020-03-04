using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Payments.Contracts.Events;
using ModularMonolith.Registrations.Contracts.Commands;

namespace ModularMonolith.Registrations.EventHandlers
{
    public class OnPaymentCompleted : INotificationHandler<PaymentCompleted>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IMediator _mediator;

        public OnPaymentCompleted(IRegistrationRepository registrationRepository, IMediator mediator)
        {
            _registrationRepository = registrationRepository;
            _mediator = mediator;
        }

        public async Task Handle(PaymentCompleted notification, CancellationToken cancellationToken)
        {
            await _registrationRepository.GetIdentifierForCorrelation(notification.CorrelationId)
                .Bind(async registrationId =>
                    await _mediator.Send(new MarkRegistrationAsPaid(registrationId), cancellationToken));
        }
    }
}