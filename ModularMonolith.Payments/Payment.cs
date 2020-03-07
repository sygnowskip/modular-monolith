using System;
using CSharpFunctionalExtensions;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments
{
    public class Payment
    {
        private Payment(Guid correlationId)
        {
            CorrelationId = correlationId;
            Status = PaymentStatus.New;
        }

        public PaymentId Id { get; private set; }
        public Guid CorrelationId { get; private set; }
        public PaymentStatus Status { get; private set; }

        public Result CompletePayment()
        {
            Status = PaymentStatus.Completed;
            return Result.Ok();
        }

        public static Result<Payment> Create(Guid correlationId)
        { 
            return Result.Ok(new Payment(correlationId));
        }
    }
}