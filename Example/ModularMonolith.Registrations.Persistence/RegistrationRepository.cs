using System;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Registrations.Domain;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Persistence
{
    internal class RegistrationRepository : IRegistrationRepository
    {
        private readonly IRegistrationsDbContext _dbContext;

        public RegistrationRepository(IRegistrationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Registration>> SaveAsync(Registration aggregate)
        {
            _dbContext.Registrations.Add(aggregate);
            await _dbContext.SaveChangesAsync();
            return Result.Ok(aggregate);
        }

        public async Task<Result<Registration>> GetAsync(RegistrationId identifier)
        {
            return Maybe<Registration>.From(
                    await _dbContext.Registrations.SingleOrDefaultAsync(r => r.Id == identifier))
                .ToResult(RegistrationRepositoryErrors.UnableToFindRegistration.Build());
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