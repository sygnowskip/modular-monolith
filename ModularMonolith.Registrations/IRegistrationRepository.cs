using System;
using CSharpFunctionalExtensions;
using ModularMonolith.Common.Aggregates;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations
{
    internal interface IRegistrationRepository : IAggregateRootRepository<Registration, RegistrationId>
    {
        Result<RegistrationId> GetIdentifierForCorrelation(Guid correlationId);
    }
}