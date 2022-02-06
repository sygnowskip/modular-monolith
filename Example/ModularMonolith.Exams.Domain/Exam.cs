using Hexure;
using Hexure.Deleting;
using Hexure.Events;
using Hexure.Time;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Exams.Language;
using ModularMonolith.Language;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;
using Stateless;

namespace ModularMonolith.Exams.Domain
{
    public partial class Exam : AggregateRoot<ExamId, ExamStatus, ExamActions>, IDeletableAggregate
    {
        private Exam(ISystemTimeProvider systemTimeProvider) : base(systemTimeProvider)
        {
        }

        private Exam(SubjectId subjectId, LocationId locationId, UtcDateTime examDateTime, Capacity capacity,
            UtcDate registrationStartDate, UtcDate registrationEndDate, ISystemTimeProvider systemTimeProvider)
            : this(systemTimeProvider)
        {
            LocationId = locationId;
            Capacity = capacity;
            Booked = Booked.Zero;
            SubjectId = subjectId;
            RegistrationStartDate = registrationStartDate;
            RegistrationEndDate = registrationEndDate;
            ExamDateTime = examDateTime;
            Status = ExamStatus.Planned;
        }

        public LocationId LocationId { get; }
        public SubjectId SubjectId { get; }
        public UtcDateTime ExamDateTime { get; }
        
        public Capacity Capacity { get; private set; }
        public Booked Booked { get; private set; }
        public UtcDate RegistrationStartDate { get; private set; }
        public UtcDate RegistrationEndDate { get; private set; }

        public bool IsDeleted => StateMachine.IsInState(ExamStatus.Deleted);
    }
}