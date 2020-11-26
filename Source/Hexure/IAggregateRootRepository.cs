using System.Threading.Tasks;
using Hexure.Results;

namespace Hexure
{
    //TODO: Value tasks?
    public interface IAggregateRootRepository<TAggregate, TIdentifier>
        where TAggregate : IAggregateRoot<TIdentifier>
    {
        Task<Result<TAggregate>> SaveAsync(TAggregate aggregate);
        Task<Result<TAggregate>> GetAsync(TIdentifier identifier);
        Task<Result> Delete(TAggregate aggregate);
    }
}
