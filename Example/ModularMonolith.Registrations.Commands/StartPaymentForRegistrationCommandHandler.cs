using System.Threading;
using System.Threading.Tasks;
using Hexure.MediatR;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using MediatR;
using ModularMonolith.Payments.Language;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Commands
{
    internal class StartPaymentForRegistration : ICommandRequest
    {
        public StartPaymentForRegistration(RegistrationId id, PaymentId paymentId)
        {
            Id = id;
            PaymentId = paymentId;
        }

        public RegistrationId Id { get; }
        public PaymentId PaymentId { get; }
    }

    internal class StartPaymentForRegistrationCommandHandler : IRequestHandler<StartPaymentForRegistration, Result>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ISystemTimeProvider _systemTimeProvider;

        public StartPaymentForRegistrationCommandHandler(IRegistrationRepository registrationRepository, ISystemTimeProvider systemTimeProvider)
        {
            _registrationRepository = registrationRepository;
            _systemTimeProvider = systemTimeProvider;
        }

        public async Task<Result> Handle(StartPaymentForRegistration request, CancellationToken cancellationToken)
        {
            return await _registrationRepository.GetAsync(request.Id)
                .ToResult(RegistrationRepositoryErrors.UnableToFindRegistration.Build())
                .OnSuccess(registration => registration.PaymentStarted(request.PaymentId, _systemTimeProvider));

        }
    }
}