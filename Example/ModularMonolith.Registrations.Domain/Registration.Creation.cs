using Hexure.Identifiers.Guid;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using ModularMonolith.Registrations.Events;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.Registrations.Domain
{
    public partial class Registration
    {
        public static Result<Registration> Create(Candidate candidate, ISystemTimeProvider systemTimeProvider)
        {
            return Result.Create(candidate != null, RegistrationErrors.CandidateCannotBeEmpty.Build())
                .OnSuccess(() => new Registration(Identifier.New<RegistrationId>(), candidate))
                .OnSuccess(registration => registration.RaiseEvent(new RegistrationCreated(registration.Id, candidate, systemTimeProvider.UtcNow)));
        }
    }
}