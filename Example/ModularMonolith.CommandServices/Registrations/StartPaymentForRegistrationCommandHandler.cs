using System.Threading;
using System.Threading.Tasks;
using Hexure.MediatR;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using MediatR;
using ModularMonolith.Registrations.Domain;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.CommandServices.Registrations
{
    public class StartPaymentForRegistration : ICommandRequest
    {
        public StartPaymentForRegistration(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }

    public class StartPaymentForRegistrationCommandHandler : IRequestHandler<StartPaymentForRegistration, Result>
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
                .OnSuccess(registration => registration.PaymentStarted(_systemTimeProvider));

        }
    }
}