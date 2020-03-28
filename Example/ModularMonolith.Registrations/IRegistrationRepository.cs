using System;
using Hexure;
using Hexure.Results;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations
{
    public static class RegistrationRepositoryErrors
    {
        public static Error.ErrorType UnableToFindRegistration = new Error.ErrorType(nameof(UnableToFindRegistration), "Unable to find registration for specified identifier");
    }

    internal interface IRegistrationRepository : IAggregateRootRepository<Registration, RegistrationId>
    {
        Result<RegistrationId> GetIdentifierForCorrelation(Guid correlationId);
    }
}