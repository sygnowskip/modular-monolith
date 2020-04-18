using System.Collections.Generic;
using System.Linq;

namespace Hexure.Events
{
    public abstract class Entity : IEntityWithDomainEvents
    {
        private readonly Queue<IEvent> _domainEvents = new Queue<IEvent>();
        public bool HasDomainEvents => _domainEvents.Any();

        public IEnumerable<IEvent> FlushDomainEvents()
        {
            while (_domainEvents.Count > 0)
            {
                yield return _domainEvents.Dequeue();
            }
        }

        protected void RaiseEvent(IEvent domainEvent)
        {
            _domainEvents.Enqueue(domainEvent);
        }
    }
}