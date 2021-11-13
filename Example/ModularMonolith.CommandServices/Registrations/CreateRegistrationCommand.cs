using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using MediatR;
using ModularMonolith.Registrations.Domain;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.CommandServices.Registrations
{
    public class CreateRegistrationCommand : IRequest<Result<RegistrationId>>
    {
        public CreateRegistrationCommand(Candidate candidate)
        {
            Candidate = candidate;
        }

        public Candidate Candidate { get; }
    }

    public class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand, Result<RegistrationId>>
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
                .OnSuccess(async registration => await _registrationRepository.SaveAsync(registration))
                .OnSuccess(registration => registration.Id);
        }
    }
}