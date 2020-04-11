using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Time;

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
        private readonly ISystemTimeProvider _systemTimeProvider;

        public MarkRegistrationAsPaidCommandHandler(IRegistrationRepository registrationRepository, ISystemTimeProvider systemTimeProvider)
        {
            _registrationRepository = registrationRepository;
            _systemTimeProvider = systemTimeProvider;
        }

        public async Task<Result> Handle(MarkRegistrationAsPaid request, CancellationToken cancellationToken)
        {
            return await _registrationRepository.GetAsync(request.Id)
                .ToResult(RegistrationRepositoryErrors.UnableToFindRegistration.Build())
                .OnSuccess(registration => registration.MarkAsPaid(_systemTimeProvider));
        }
    }
}