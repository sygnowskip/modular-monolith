using System.Collections.Generic;

namespace Hexure.Events
{
    public interface IEntityWithDomainEvents
    {
        bool HasDomainEvents { get; }

        IEnumerable<IEvent> FlushDomainEvents();
    }
}