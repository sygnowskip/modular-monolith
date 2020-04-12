using System.Threading.Tasks;

namespace Hexure.EntityFrameworkCore
{
    public interface ITransactionProvider
    {
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}