using Hexure;
using Hexure.Events;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using ModularMonolith.Exams.Language;
using ModularMonolith.Orders.Language;
using ModularMonolith.Registrations.Events;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.Registrations.Domain
{
    public static class RegistrationErrors
    {
        public static Error.ErrorType CandidateCannotBeEmpty = new Error.ErrorType(nameof(CandidateCannotBeEmpty), "Candidate cannot be empty");
        public static Error.ErrorType PaymentInProgress = new Error.ErrorType(nameof(PaymentInProgress), "There is already a payment in progress");
        public static Error.ErrorType NoPayment = new Error.ErrorType(nameof(NoPayment), "There is no payment to complete");
    }

    public partial class Registration : Entity, IAggregateRoot<RegistrationId>
    {
        private Registration() { }

        private Registration(RegistrationId id, Candidate candidate)
        {
            Id = id;
            Status = RegistrationStatus.New;
            Candidate = candidate;
        }

        public RegistrationId Id { get; private set; }
        public ExamId ExamId { get; private set; }
        public OrderId OrderId { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public Candidate Candidate { get; private set; }

        public Result PaymentStarted(ISystemTimeProvider systemTimeProvider)
        {
            return Result.Ok()
                .OnSuccess(() => Status = RegistrationStatus.AwaitingPayment)
                .OnSuccess(_ => RaiseEvent(new PaymentForRegistrationStarted(Id, systemTimeProvider.UtcNow)));
        }

        public void MarkAsPaid(ISystemTimeProvider systemTimeProvider)
        {
            Status = RegistrationStatus.Paid;
            RaiseEvent(new RegistrationPaid(Id, systemTimeProvider.UtcNow));
        }

        public void MarkAsCompleted()
        {
            Status = RegistrationStatus.Completed;
        }
    }

    public class RegistrationPayment
    {
        internal RegistrationPayment(decimal fee, string currency)
        {
            Fee = fee;
            Currency = currency;
        }

        public decimal Fee { get; private set; }
        public string Currency { get; private set; }
    }
}