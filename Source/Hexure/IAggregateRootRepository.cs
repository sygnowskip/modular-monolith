﻿using System.Threading.Tasks;
using Hexure.Results;

namespace Hexure
{
    public interface IAggregateRootRepository<TAggregate, in TIdentifier>
    {
        Task<Result> SaveAsync(TAggregate aggregate);
        Task<Maybe<TAggregate>> GetAsync(TIdentifier identifier);
        Task<Result> Delete(TAggregate aggregate);
    }
}
