using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Entites;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Payments;
using ModularMonolith.Persistence.Configurations;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    internal class MonolithDbContext : DbContext, ISerializedEventDbContext
    {
        public MonolithDbContext(DbContextOptions<MonolithDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RegistrationEntityConfiguration());
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<SerializedEventEntity> SerializedEvents { get; set; }
    }
}