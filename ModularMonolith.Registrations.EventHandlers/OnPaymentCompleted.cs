using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Payments.Contracts.Events;
using ModularMonolith.Registrations.Contracts;

namespace ModularMonolith.Registrations.EventHandlers
{
    internal class OnPaymentCompleted : INotificationHandler<PaymentCompleted>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IRegistrationApplicationService _registrationApplicationService;

        public OnPaymentCompleted(IRegistrationRepository registrationRepository, IRegistrationApplicationService registrationApplicationService)
        {
            _registrationRepository = registrationRepository;
            _registrationApplicationService = registrationApplicationService;
        }

        public async Task Handle(PaymentCompleted notification, CancellationToken cancellationToken)
        {
            await _registrationRepository.GetIdentifierForCorrelation(notification.CorrelationId)
                .Bind(async registrationId => await _registrationApplicationService.MarkAsPaid(registrationId));
        }
    }
}