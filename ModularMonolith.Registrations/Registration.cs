using CSharpFunctionalExtensions;
using ModularMonolith.Payments.Language;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations
{
    internal class Registration
    {
        private Registration(RegistrationId id, RegistrationPayment payment)
        {
            Id = id;
            Status = RegistrationStatus.New;
            Payment = payment;
        }

        public RegistrationId Id { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public RegistrationPayment Payment { get; private set; }

        public Result PaymentStarted(PaymentId paymentId)
        {
            return Payment.SetInProgressPayment(paymentId);
        }
        public void MarkAsPaid()
        {
            Status = RegistrationStatus.Paid;
            Payment.CompletePayment();
        }

        public void MarkAsCompleted()
        {
            Status = RegistrationStatus.Completed;
        }
    }

    internal class RegistrationPayment
    {
        private RegistrationPayment(decimal fee, string currency)
        {
            Fee = fee;
            Currency = currency;
        }

        public decimal Fee { get; private set; }
        public string Currency { get; private set; }
        public Maybe<PaymentId> InProgressPaymentId { get; private set; }

        public Result SetInProgressPayment(PaymentId paymentId)
        {
            if (InProgressPaymentId.HasValue)
                return Result.Failure("There is already payment in progress");

            InProgressPaymentId = paymentId;
            return Result.Ok();
        }
        public Result CompletePayment()
        {
            if (InProgressPaymentId.HasNoValue)
                return Result.Failure("There is no payment to complete");

            InProgressPaymentId = Maybe<PaymentId>.None;
            return Result.Ok();
        }
    }
}