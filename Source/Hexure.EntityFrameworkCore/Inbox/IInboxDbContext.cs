using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore.Inbox.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hexure.EntityFrameworkCore.Inbox
{
    public interface IInboxDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        DbSet<ProcessedEventEntity> ProcessedEvents { get; }
    }
}