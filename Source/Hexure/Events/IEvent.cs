using System;
using MediatR;

namespace Hexure.Events
{
    public interface IEvent
    {
        DateTime PublishedOn { get; }
    }
}