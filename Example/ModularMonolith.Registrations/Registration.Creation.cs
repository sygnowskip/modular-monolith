using Hexure.Results;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.ValueObjects;

namespace ModularMonolith.Registrations
{
    internal partial class Registration
    {
        public static Result<Registration> Create(Candidate candidate)
        {
            return Result.Ok(new Registration(new RegistrationId(), candidate));
        }
    }
}