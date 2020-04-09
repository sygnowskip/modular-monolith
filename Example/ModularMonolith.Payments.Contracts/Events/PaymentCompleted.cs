using System;
using Hexure.Events;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments.Contracts.Events
{
    public class PaymentCompleted : IEvent
    {
        public PaymentCompleted(PaymentId id, Guid correlationId, DateTime publishedOn)
        {
            Id = id;
            CorrelationId = correlationId;
            PublishedOn = publishedOn;
        }

        public PaymentId Id { get; }
        public Guid CorrelationId { get; }
        public DateTime PublishedOn { get; }
    }
}