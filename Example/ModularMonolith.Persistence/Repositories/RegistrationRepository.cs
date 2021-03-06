﻿using System;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Registrations;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Persistence.Repositories
{
    internal class RegistrationRepository : IRegistrationRepository
    {
        private readonly MonolithDbContext _dbContext;

        public RegistrationRepository(MonolithDbContext dbContext)
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