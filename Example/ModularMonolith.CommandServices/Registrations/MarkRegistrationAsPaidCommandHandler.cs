using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using MediatR;
using ModularMonolith.Registrations.Domain;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.CommandServices.Registrations
{
    public class MarkRegistrationAsPaid : IRequest<Result>
    {
        public MarkRegistrationAsPaid(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }

    public class MarkRegistrationAsPaidCommandHandler : IRequestHandler<MarkRegistrationAsPaid, Result>
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
                .OnSuccess(registration => registration.MarkAsPaid(_systemTimeProvider));
        }
    }
}