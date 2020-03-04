using ModularMonolith.Common.Aggregates;
using ModularMonolith.Payments.Contracts;

namespace ModularMonolith.Payments
{
    public interface IPaymentRepository : IAggregateRootRepository<Payment, PaymentId>
    {
    }
}