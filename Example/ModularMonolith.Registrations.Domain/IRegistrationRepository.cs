using Hexure;
using Hexure.Results;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Domain
{
    public static class RegistrationRepositoryErrors
    {
        public static Error.ErrorType UnableToFindRegistration = new Error.ErrorType(nameof(UnableToFindRegistration), "Unable to find registration for specified identifier");
    }

    public interface IRegistrationRepository : IAggregateRootRepository<Registration, RegistrationId>
    {
    }
}