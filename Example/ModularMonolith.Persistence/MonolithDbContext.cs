using System;
using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Collecting;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.EntityFrameworkCore.Events.Publishing;
using Hexure.Events.Publishing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Payments;
using ModularMonolith.Persistence.Configurations;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    internal class MonolithDbContext : DbContext, ISerializedEventDbContext, ITransactionProvider
    {
        private readonly IEventCollector _eventCollector;
        private readonly IServiceProvider _serviceProvider;
        public MonolithDbContext(DbContextOptions<MonolithDbContext> options, IEventCollector eventCollector, IServiceProvider serviceProvider) : base(options)
        {
            _eventCollector = eventCollector;
            _serviceProvider = serviceProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RegistrationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SerializedEventEntityConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await PublishDomainEvents();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private Task PublishDomainEvents()
        {
            return _serviceProvider.GetRequiredService<IEventPublisher>()
                .Publish(_eventCollector.Collect(ChangeTracker));
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