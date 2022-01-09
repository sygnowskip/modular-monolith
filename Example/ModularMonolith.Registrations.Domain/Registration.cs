using Hexure;
using Hexure.Events;
using Hexure.Time;
using ModularMonolith.Exams.Language;
using ModularMonolith.Orders.Language;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;
using Stateless;

namespace ModularMonolith.Registrations.Domain
{
    public partial class Registration : Entity, IAggregateRoot<RegistrationId>
    {
        private readonly ISystemTimeProvider _systemTimeProvider;

        private readonly StateMachine<RegistrationStatus, RegistrationActions> _stateMachine;

        public Registration(ISystemTimeProvider systemTimeProvider)
        {
            _systemTimeProvider = systemTimeProvider;
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

        public RegistrationId Id { get; private set; }
        public ExternalRegistrationId ExternalId { get; private set; }
        public ExamId ExamId { get; private set; }
        public OrderId OrderId { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public Candidate Candidate { get; private set; }
    }
}