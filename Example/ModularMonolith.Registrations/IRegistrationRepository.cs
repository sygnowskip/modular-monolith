using System;
using CSharpFunctionalExtensions;
using Hexure;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations
{
    internal interface IRegistrationRepository : IAggregateRootRepository<Registration, RegistrationId>
    {
        Result<RegistrationId> GetIdentifierForCorrelation(Guid correlationId);
    }
}