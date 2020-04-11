using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;
using ModularMonolith.Time;

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
        private readonly ISystemTimeProvider _systemTimeProvider;

        public CreateRegistrationCommandHandler(IRegistrationRepository registrationRepository, ISystemTimeProvider systemTimeProvider)
        {
            _registrationRepository = registrationRepository;
            _systemTimeProvider = systemTimeProvider;
        }

        public async Task<Result<RegistrationId>> Handle(CreateRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await Registration.Create(request.Candidate, _systemTimeProvider)
                .OnSuccess(async registration => await _registrationRepository.SaveAsync(registration));
        }
    }
}