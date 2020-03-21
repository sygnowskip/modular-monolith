using Hexure;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments
{
    internal interface IPaymentRepository : IAggregateRootRepository<Payment, PaymentId>
    {
    }
}