using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.Events;
using Hexure.Events.Collecting;
using Hexure.Events.Serialization;
using Hexure.Results.Extensions;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Exams.Persistence.Configurations;
using ModularMonolith.Persistence.Configurations;

namespace ModularMonolith.Persistence
{
    internal partial class MonolithDbContext : DbContext, ISerializedEventDbContext, ITransactionProvider
    {
        private readonly IEventCollector _eventCollector;
        private readonly IEventSerializer _eventSerializer;

        public MonolithDbContext(DbContextOptions<MonolithDbContext> options, IEventCollector eventCollector,
            IEventSerializer eventSerializer) : base(options)
        {
            _eventCollector = eventCollector;
            _eventSerializer = eventSerializer;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExamConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocationConfiguration).Assembly);
            modelBuilder.ApplyConfiguration(new RegistrationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SerializedEventEntityConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await PublishDomainEventsAsync();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task PublishDomainEventsAsync()
        {
            var events = _eventCollector
                .Collect(ChangeTracker.Entries<IEntityWithDomainEvents>()
                    .Select(entry => entry.Entity)
                    .ToList());

            foreach (var @event in events)
            {
                await _eventSerializer.Serialize(@event)
                    .OnSuccess(SerializedEventEntity.Create)
                    .OnSuccess(serializedEvent => SerializedEvents.Add(serializedEvent))
                    .OnFailure(errors =>
                        throw new InvalidOperationException(
                            $"Unable to publish domain event due to: {string.Join(", ", errors.Select(e => e.Message))}"));
            }
        }

        public Task BeginTransactionAsync()
        {
            return Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await SaveChangesAsync();
            Database.CommitTransaction();
        }

        public Task RollbackTransactionAsync()
        {
            Database.RollbackTransaction();
            return Task.CompletedTask;
        }
    }
}