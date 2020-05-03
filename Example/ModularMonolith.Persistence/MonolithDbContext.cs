using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Collecting;
using Hexure.EntityFrameworkCore.Events.Entites;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Payments;
using ModularMonolith.Persistence.Configurations;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    internal class MonolithDbContext : DbContext, ISerializedEventDbContext, ITransactionProvider
    {
        private readonly IEventCollector _eventCollector;
        public MonolithDbContext(DbContextOptions<MonolithDbContext> options, IEventCollector eventCollector) : base(options)
        {
            _eventCollector = eventCollector;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RegistrationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SerializedEventEntityConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await SerializedEvents.AddRangeAsync(_eventCollector.Collect(ChangeTracker), cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<SerializedEventEntity> SerializedEvents { get; set; }
        public Task BeginTransactionAsync()
        {
            return Database.BeginTransactionAsync();
        }

        public Task CommitTransactionAsync()
        {
            Database.CommitTransaction();
            return Task.CompletedTask;
        }

        public Task RollbackTransactionAsync()
        {
            Database.RollbackTransaction();
            return Task.CompletedTask;
        }
    }
}