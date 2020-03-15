using ModularMonolith.Common.Aggregates;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments
{
    internal interface IPaymentRepository : IAggregateRootRepository<Payment, PaymentId>
    {
    }
}