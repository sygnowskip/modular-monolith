using System;
using MediatR;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments.Contracts.Events
{
    public class PaymentCompleted : INotification
    {
        public PaymentCompleted(PaymentId id, Guid correlationId)
        {
            Id = id;
            CorrelationId = correlationId;
        }

        public PaymentId Id { get; }
        public Guid CorrelationId { get; }
    }
}