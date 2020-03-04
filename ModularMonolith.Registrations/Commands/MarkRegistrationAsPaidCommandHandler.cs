using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Registrations.Contracts.Commands;
using ModularMonolith.Registrations.Contracts.Events;

namespace ModularMonolith.Registrations.Commands
{
    public class MarkRegistrationAsPaidCommandHandler : IRequestHandler<MarkRegistrationAsPaid, Result>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IMediator _mediator;

        public MarkRegistrationAsPaidCommandHandler(IRegistrationRepository registrationRepository, IMediator mediator)
        {
            _registrationRepository = registrationRepository;
            _mediator = mediator;
        }

        public async Task<Result> Handle(MarkRegistrationAsPaid request, CancellationToken cancellationToken)
        {
            var registrationResult = await _registrationRepository.GetAsync(request.Id)
                .ToResult($"Unable to find registration with id: {request.Id}");

            return await registrationResult
                .Tap(async paymentId =>
                {
                    registrationResult.Value.MarkAsPaid();

                    //TODO: Event should be on aggregate
                    await _mediator.Publish(new RegistrationPaid(registrationResult.Value.Id), cancellationToken);
                });
        }
    }
}