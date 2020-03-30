using Hexure.Identifiers.Guid;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.ValueObjects;

namespace ModularMonolith.Registrations
{
    internal partial class Registration
    {
        public static Result<Registration> Create(Candidate candidate)
        {
            return Result.Create(candidate != null, RegistrationErrors.CandidateCannotBeEmpty.Build())
                .OnSuccess(() => new Registration(Identifier.New<RegistrationId>(), candidate));
        }
    }
}