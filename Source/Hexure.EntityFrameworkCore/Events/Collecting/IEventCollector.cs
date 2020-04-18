using System.Collections.Generic;
using System.Linq;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.Events;
using Hexure.Events.Serialization;
using Hexure.Results.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hexure.EntityFrameworkCore.Events.Collecting
{
    public interface IEventCollector
    {
        IEnumerable<SerializedEventEntity> Collect(ChangeTracker changeTracker);
    }

    public class EventCollector : IEventCollector
    {
        private readonly IEventSerializer _eventSerializer;

        public EventCollector(IEventSerializer eventSerializer)
        {
            _eventSerializer = eventSerializer;
        }

        public IEnumerable<SerializedEventEntity> Collect(ChangeTracker changeTracker)
        {
            var events = changeTracker.Entries<IEntityWithDomainEvents>()
                .Where(e => e.Entity.HasDomainEvents)
                .SelectMany(e => e.Entity.FlushDomainEvents())
                .ToList();

            foreach (var @event in events)
            {
                yield return _eventSerializer.Serialize(@event)
                    .OnSuccess(SerializedEventEntity.Create)
                    .Value;
            }
        }
    }
}