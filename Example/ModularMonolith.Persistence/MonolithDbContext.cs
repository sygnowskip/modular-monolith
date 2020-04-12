using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Entites;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Payments;
using ModularMonolith.Persistence.Configurations;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    internal class MonolithDbContext : DbContext, ISerializedEventDbContext, ITransactionProvider
    {
        public MonolithDbContext(DbContextOptions<MonolithDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RegistrationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SerializedEventEntityConfig());
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<SerializedEventEntity> SerializedEvents { get; set; }
        public Task BeginTransaction()
        {
            return Database.BeginTransactionAsync();
        }

        public Task CommitTransaction()
        {
            Database.CommitTransaction();
            return Task.CompletedTask;
        }

        public Task RollbackTransaction()
        {
            Database.RollbackTransaction();
            return Task.CompletedTask;
        }
    }
}