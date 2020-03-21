using Microsoft.EntityFrameworkCore;
using ModularMonolith.Payments;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    internal class MonolithDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}