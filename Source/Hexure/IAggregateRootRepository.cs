using System.Threading.Tasks;
using Hexure.Results;

namespace Hexure
{
    public interface IAggregateRootRepository<TAggregate, TIdentifier>
        where TAggregate : IAggregateRoot<TIdentifier>
    {
        Task<Result<TIdentifier>> SaveAsync(TAggregate aggregate);
        Task<Maybe<TAggregate>> GetAsync(TIdentifier identifier);
        Task<Result> Delete(TAggregate aggregate);
    }
}
