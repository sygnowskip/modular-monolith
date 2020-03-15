using System;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Persistence.Read.Entities
{
    public class Payment
    {
        public PaymentId Id { get; set; }
        public PaymentStatus Status { get; set; }
        public Guid CorrelationId { get; set; }
    }
}