using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.Events;
using Hexure.Events.Collecting;
using Hexure.Events.Serialization;
using Hexure.Results.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Hexure.EntityFrameworkCore.Events
{
    public class PublishDomainEventsOnSaveChangesInterceptor : ISaveChangesInterceptor
    {
        private readonly IEventCollector _eventCollector;
        private readonly IEventSerializer _eventSerializer;

        public PublishDomainEventsOnSaveChangesInterceptor(IEventCollector eventCollector, IEventSerializer eventSerializer)
        {
            _eventCollector = eventCollector;
            _eventSerializer = eventSerializer;
        }

        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            PublishDomainEvents(eventData.Context);
            return result;
        }

        public async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await PublishDomainEventsAsync(eventData.Context);
            return result;
        }

        private void PublishDomainEvents(DbContext dbContext)
        {
            var events = GetEvents(dbContext.ChangeTracker);
            if (!events.Any())
                return;

            var serializedEvents = SerializeEvents(events);
            dbContext.Set<SerializedEventEntity>().AddRange(serializedEvents);
        }

        private Task PublishDomainEventsAsync(DbContext dbContext)
        {
            var events = GetEvents(dbContext.ChangeTracker);
            if (!events.Any())
                return Task.CompletedTask;

            var serializedEvents = SerializeEvents(events);
            return dbContext.Set<SerializedEventEntity>().AddRangeAsync(serializedEvents);
        }

        private ICollection<SerializedEventEntity> SerializeEvents(ICollection<IEvent> events)
        {
            var serializedEvents = new List<SerializedEventEntity>();
            foreach (var @event in events)
            {
                _eventSerializer.Serialize(@event)
                    .OnSuccess(SerializedEventEntity.Create)
                    .OnSuccess(entity => serializedEvents.Add(entity))
                    .OnFailure(errors =>
                        throw new InvalidOperationException(
                            $"Unable to publish domain event due to: {string.Join(", ", errors.Select(e => e.Message))}"));
            }
            
            return serializedEvents;
        }
        
        private ICollection<IEvent> GetEvents(ChangeTracker changeTracker)
        {
            return _eventCollector
                .Collect(changeTracker.Entries<IEntityWithDomainEvents>()
                    .Select(entry => entry.Entity)
                    .ToList())
                .ToList();
        }

        #region default implementations

        public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            return result;
        }

        public void SaveChangesFailed(DbContextErrorEventData eventData)
        {
        }

        public ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return new ValueTask<int>(result);
        }

        public Task SaveChangesFailedAsync(DbContextErrorEventData eventData,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}