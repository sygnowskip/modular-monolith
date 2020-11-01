using Hexure;
using Hexure.Events;
using Hexure.Results;
using Hexure.Time;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Exams.Events;
using ModularMonolith.Exams.Language;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;
using Stateless;

namespace ModularMonolith.Exams.Domain
{
    public partial class Exam : Entity, IAggregateRoot<ExamId>
    {
        private readonly ISystemTimeProvider _systemTimeProvider;

        private readonly StateMachine<ExamStatus, ExamActions> _stateMachine;
        

        protected Exam(ISystemTimeProvider systemTimeProvider)
        {
            _systemTimeProvider = systemTimeProvider;
        }
        
        private Exam(SubjectId subjectId, LocationId locationId, UtcDateTime examDateTime, Capacity capacity,
            UtcDate registrationStartDate, UtcDate registrationEndDate, ISystemTimeProvider systemTimeProvider)
        {
            LocationId = locationId;
            Capacity = capacity;
            SubjectId = subjectId;
            RegistrationStartDate = registrationStartDate;
            RegistrationEndDate = registrationEndDate;
            _systemTimeProvider = systemTimeProvider;
            ExamDateTime = examDateTime;
            Status = ExamStatus.Planned;

            _stateMachine = new StateMachine<ExamStatus, ExamActions>(() => Status, status => Status = status);
            ConfigureStateMachine();
        }

        private void ConfigureStateMachine()
        {
            _stateMachine.Configure(ExamStatus.Planned)
                .Permit(ExamActions.OpenForRegistration, ExamStatus.AvailableForRegistration);

            _stateMachine.Configure(ExamStatus.AvailableForRegistration)
                .OnEntry(() => RaiseEvent(new ExamAvailable(Id, _systemTimeProvider.UtcNow)))
                .Permit(ExamActions.CloseRegistration, ExamStatus.ClosedForRegistration);

            _stateMachine.Configure(ExamStatus.ClosedForRegistration)
                .OnEntry(() => RaiseEvent(new ExamClosed(Id, _systemTimeProvider.UtcNow)))
                .Permit(ExamActions.MarkAsDone, ExamStatus.TookPlace);

            _stateMachine.Configure(ExamStatus.TookPlace)
                .OnEntry(() => RaiseEvent(new ExamTookPlace(Id, _systemTimeProvider.UtcNow)));
        }

        public ExamId Id { get; }
        public LocationId LocationId { get; }
        public Capacity Capacity { get; }
        public SubjectId SubjectId { get; }

        public UtcDate RegistrationStartDate { get; }
        public UtcDate RegistrationEndDate { get; }

        public UtcDateTime ExamDateTime { get; }

        public ExamStatus Status { get; private set; }

        public Result OpenForRegistration(ISystemTimeProvider systemTimeProvider)
        {
            if (!_stateMachine.CanFire(ExamActions.OpenForRegistration) || RegistrationStartDate.Value >= systemTimeProvider.UtcNow)
                return Result.Fail(ExamErrors.UnableToOpenForRegistration.Build());
            
            _stateMachine.Fire(ExamActions.OpenForRegistration);
            return Result.Ok();
        }

        public Result CloseRegistration(ISystemTimeProvider systemTimeProvider)
        {
            if (!_stateMachine.CanFire(ExamActions.CloseRegistration) || RegistrationEndDate.Value >= systemTimeProvider.UtcNow)
                return Result.Fail(ExamErrors.UnableToCloseRegistration.Build());
            
            _stateMachine.Fire(ExamActions.CloseRegistration);
            return Result.Ok();
        }

        public Result MarkAsDone(ISystemTimeProvider systemTimeProvider)
        {
            if (!_stateMachine.CanFire(ExamActions.MarkAsDone) || ExamDateTime.Value >= systemTimeProvider.UtcNow)
                return Result.Fail(ExamErrors.UnableToCloseRegistration.Build());
            
            _stateMachine.Fire(ExamActions.MarkAsDone);
            return Result.Ok();
        }
    }

    public static class ExamErrors
    {
        public static readonly Error.ErrorType UnableToOpenForRegistration =
            new Error.ErrorType(nameof(UnableToOpenForRegistration), "Unable to open for registration");
        
        public static readonly Error.ErrorType UnableToCloseRegistration =
            new Error.ErrorType(nameof(UnableToCloseRegistration), "Unable to close registration");
        
        public static readonly Error.ErrorType UnableToMarkAsDone =
            new Error.ErrorType(nameof(UnableToMarkAsDone), "Unable to mark as done");
    }
}