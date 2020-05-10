using System.Collections.Generic;
using System.Linq;
using Hexure.Events;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hexure.EntityFrameworkCore.Events.Collecting
{
    public interface IEventCollector
    {
        IEnumerable<IEvent> Collect(ChangeTracker changeTracker);
    }

    public class EventCollector : IEventCollector
    {
        public IEnumerable<IEvent> Collect(ChangeTracker changeTracker)
        {
            return changeTracker.Entries<IEntityWithDomainEvents>()
                .Where(e => e.Entity.HasDomainEvents)
                .SelectMany(e => e.Entity.FlushDomainEvents())
                .ToList();
        }
    }
}