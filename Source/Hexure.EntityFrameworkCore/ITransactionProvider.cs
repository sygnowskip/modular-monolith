using System.Threading.Tasks;

namespace Hexure.EntityFrameworkCore
{
    public interface ITransactionProvider
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}