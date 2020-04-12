using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore.Events.Entites;
using Microsoft.EntityFrameworkCore;

namespace Hexure.EntityFrameworkCore.Events
{
    public interface ISerializedEventDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        DbSet<SerializedEventEntity> SerializedEvents { get; }
    }
}