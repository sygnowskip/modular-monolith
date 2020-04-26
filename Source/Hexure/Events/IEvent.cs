using System;

namespace Hexure.Events
{
    public interface IEvent
    {
        DateTime PublishedOn { get; }
    }
}