using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
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
            return await _registrationRepository.GetAsync(request.Id)
                .ToResult(RegistrationRepositoryErrors.UnableToFindRegistration.Build())
                .OnSuccess(async registration =>
                {

                    registration.MarkAsPaid();

                    //TODO: Event should be on aggregate
                    await _mediator.Publish(new RegistrationPaid(registration.Id), cancellationToken);
                });
        }
    }
}