using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Registrations.Contracts.Events;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Commands
{
    internal class MarkRegistrationAsPaid : IRequest<Result>
    {
        public MarkRegistrationAsPaid(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }

    internal class MarkRegistrationAsPaidCommandHandler : IRequestHandler<MarkRegistrationAsPaid, Result>
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