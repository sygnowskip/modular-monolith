using Hexure;
using Hexure.Events;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using ModularMonolith.Payments.Language;
using ModularMonolith.Registrations.Contracts.Events;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.Registrations
{
    public static class RegistrationErrors
    {
        public static Error.ErrorType CandidateCannotBeEmpty = new Error.ErrorType(nameof(CandidateCannotBeEmpty), "Candidate cannot be empty");
        public static Error.ErrorType PaymentInProgress = new Error.ErrorType(nameof(PaymentInProgress), "There is already a payment in progress");
        public static Error.ErrorType NoPayment = new Error.ErrorType(nameof(NoPayment), "There is no payment to complete");
    }

    internal partial class Registration : Entity, IAggregateRoot<RegistrationId>
    {
        private Registration() { }

        private Registration(RegistrationId id, Candidate candidate)
        {
            Id = id;
            Status = RegistrationStatus.New;
            Candidate = candidate;
        }

        public RegistrationId Id { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public Candidate Candidate { get; private set; }
        public RegistrationPayment Payment { get; private set; }

        public Result PaymentStarted(PaymentId paymentId)
        {
            return Payment.SetInProgressPayment(paymentId)
                .OnSuccess(() => RaiseEvent(new PaymentForRegistrationStarted(Id)));
        }

        public void MarkAsPaid(ISystemTimeProvider systemTimeProvider)
        {
            Status = RegistrationStatus.Paid;
            Payment.CompletePayment();
            RaiseEvent(new RegistrationPaid(Id, systemTimeProvider.UtcNow));
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
                return Result.Fail(RegistrationErrors.PaymentInProgress.Build());

            InProgressPaymentId = paymentId;
            return Result.Ok();
        }
        public Result CompletePayment()
        {
            if (InProgressPaymentId.HasNoValue)
                return Result.Fail(RegistrationErrors.NoPayment.Build());

            InProgressPaymentId = Maybe<PaymentId>.None;
            return Result.Ok();
        }
    }
}