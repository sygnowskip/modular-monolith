using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Payments.Language;
using ModularMonolith.Registrations.Contracts.Events;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Commands
{
    internal class StartPaymentForRegistration : IRequest<Result>
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
        private readonly IMediator _mediator;

        public StartPaymentForRegistrationCommandHandler(IRegistrationRepository registrationRepository, IMediator mediator)
        {
            _registrationRepository = registrationRepository;
            _mediator = mediator;
        }

        public async Task<Result> Handle(StartPaymentForRegistration request, CancellationToken cancellationToken)
        {
            return await _registrationRepository.GetAsync(request.Id)
                .ToResult(RegistrationRepositoryErrors.UnableToFindRegistration.Build())
                .OnSuccess(async registration =>
                {
                    registration.PaymentStarted(request.PaymentId);

                    //TODO: Event should be on aggregate
                    await _mediator.Publish(new PaymentForRegistrationStarted(registration.Id), cancellationToken);
                });

        }
    }
}