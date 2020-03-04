using System;
using MediatR;

namespace ModularMonolith.Payments.Contracts.Events
{
    public class PaymentStarted : INotification
    {
        public PaymentStarted(PaymentId id, Guid correlationId)
        {
            Id = id;
            CorrelationId = correlationId;
        }

        public PaymentId Id { get; }
        public Guid CorrelationId { get; }
    }
}