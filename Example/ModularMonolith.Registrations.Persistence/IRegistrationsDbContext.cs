using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Registrations.Domain;

namespace ModularMonolith.Registrations.Persistence
{
    public interface IRegistrationsDbContext
    {
        DbSet<Registration> Registrations { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}