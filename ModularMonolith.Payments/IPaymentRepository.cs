using ModularMonolith.Common.Aggregates;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments
{
    public interface IPaymentRepository : IAggregateRootRepository<Payment, PaymentId>
    {
    }
}