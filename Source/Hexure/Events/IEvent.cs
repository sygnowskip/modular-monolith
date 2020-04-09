using System;
using MediatR;

namespace Hexure.Events
{
    public interface IEvent : INotification
    {
        DateTime PublishedOn { get; }
    }
}