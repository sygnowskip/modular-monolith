namespace ModularMonolith.Registrations.EventHandlers
{
    /*internal class OnPaymentCompleted : INotificationHandler<PaymentCompleted>
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
            var identifierResult = _registrationRepository.GetIdentifierForCorrelation(notification.CorrelationId)
                .OnSuccess(async registrationId => await _registrationApplicationService.MarkAsPaid(registrationId));
        }
    }*/
}