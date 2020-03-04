using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Payments.Contracts.Commands;
using ModularMonolith.Registrations.Contracts.Commands;
using ModularMonolith.Registrations.Contracts.Events;

namespace ModularMonolith.Registrations.Commands
{
    public class StartPaymentForRegistrationCommandHandler : IRequestHandler<StartPaymentForRegistration, Result>
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
            var registrationResult = await _registrationRepository.GetAsync(request.Id)
                .ToResult($"Unable to find registration with id: {request.Id}");

            return await registrationResult
                .Bind(async registration => await _mediator.Send(new StartPayment(registration.Id.Identifier), cancellationToken))
                .Tap(async paymentId =>
                {
                    registrationResult.Value.PaymentStarted(paymentId);

                    //TODO: Event should be on aggregate
                    await _mediator.Publish(new PaymentForRegistrationStarted(registrationResult.Value.Id), cancellationToken);
                });

        }
    }
}