using System.Collections.Generic;
using MediatR;

namespace Hexure.Events
{
    public interface IEntityWithDomainEvents
    {
        bool HasDomainEvents { get; }

        IEnumerable<INotification> FlushDomainEvents();
    }
}