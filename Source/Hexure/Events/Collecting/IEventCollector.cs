using System.Collections.Generic;
using System.Linq;

namespace Hexure.Events.Collecting
{
    public interface IEventCollector
    {
        IEnumerable<IEvent> Collect(ICollection<IEntityWithDomainEvents> entities);
    }

    public class EventCollector : IEventCollector
    {
        public IEnumerable<IEvent> Collect(ICollection<IEntityWithDomainEvents> entities)
        {
            return entities
                .Where(e => e.HasDomainEvents)
                .SelectMany(e => e.FlushDomainEvents())
                .ToList();
        }
    }
}