using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.ValueObjects;

namespace ModularMonolith.Registrations.Commands
{
    internal class CreateRegistrationCommand : IRequest<Result<RegistrationId>>
    {
        public CreateRegistrationCommand(Candidate candidate)
        {
            Candidate = candidate;
        }

        public Candidate Candidate { get; }
    }

    internal class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand, Result<RegistrationId>>
    {
        private readonly IRegistrationRepository _registrationRepository;

        public CreateRegistrationCommandHandler(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<Result<RegistrationId>> Handle(CreateRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await Registration.Create(request.Candidate)
                .OnSuccess(async registration => await _registrationRepository.SaveAsync(registration));
        }
    }
}