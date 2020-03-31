using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Hexure.Events
{
    public abstract class Entity : IEntityWithDomainEvents
    {
        private readonly Queue<INotification> _domainEvents = new Queue<INotification>();
        public bool HasDomainEvents => _domainEvents.Any();

        public IEnumerable<INotification> FlushDomainEvents()
        {
            while (_domainEvents.Count > 0)
            {
                yield return _domainEvents.Dequeue();
            }
        }

        protected void RaiseEvent(INotification domainEvent)
        {
            _domainEvents.Enqueue(domainEvent);
        }
    }
}