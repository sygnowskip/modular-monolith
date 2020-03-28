using Hexure;
using Hexure.Results;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments
{
    public static class PaymentsRepositoryErrors
    {
        public static Error.ErrorType UnableToFindPayment = new Error.ErrorType(nameof(UnableToFindPayment), "Unable to find payment for specified identifier");
    }

    internal interface IPaymentRepository : IAggregateRootRepository<Payment, PaymentId>
    {
    }
}