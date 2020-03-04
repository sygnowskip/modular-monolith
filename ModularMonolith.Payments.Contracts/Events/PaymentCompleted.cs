using System;
using MediatR;

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