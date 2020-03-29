using Microsoft.EntityFrameworkCore;
using ModularMonolith.Persistence.Read.Entities;

namespace ModularMonolith.Persistence.Read
{
    public class MonolithQueryDbContext : DbContext
    {
        public MonolithQueryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RegistrationEntityConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }

        //public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}