using System;
using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Registrations;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Persistence.Repositories
{
    internal class RegistrationRepository : IRegistrationRepository
    {
        public Task<Result<RegistrationId>> SaveAsync(Registration aggregate)
        {
            throw new NotImplementedException();
        }

        public Task<Maybe<Registration>> GetAsync(RegistrationId identifier)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Delete(Registration aggregate)
        {
            throw new NotImplementedException();
        }

        public Result<RegistrationId> GetIdentifierForCorrelation(Guid correlationId)
        {
            throw new NotImplementedException();
        }
    }
}