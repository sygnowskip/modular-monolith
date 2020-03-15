using Microsoft.EntityFrameworkCore;
using ModularMonolith.Persistence.Read.Entities;

namespace ModularMonolith.Persistence.Read
{
    public class MonolithQueryDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}