using Hexure;
using Hexure.Time;
using ModularMonolith.Exams.Language;
using ModularMonolith.Orders.Language;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.Registrations.Domain
{
    public partial class Registration : AggregateRoot<RegistrationId, RegistrationStatus, RegistrationActions>
    {
        private Registration(ISystemTimeProvider systemTimeProvider) : base(systemTimeProvider)
        {
        }

        private Registration(ExternalRegistrationId externalId, ExamId examId, OrderId orderId, Candidate candidate,
            ISystemTimeProvider systemTimeProvider) : this(systemTimeProvider)
        {
            ExternalId = externalId;
            Status = RegistrationStatus.New;
            Candidate = candidate;
            ExamId = examId;
            OrderId = orderId;
        }
        public ExternalRegistrationId ExternalId { get; private set; }
        public ExamId ExamId { get; private set; }
        public OrderId OrderId { get; private set; }
        public Candidate Candidate { get; private set; }
    }
}