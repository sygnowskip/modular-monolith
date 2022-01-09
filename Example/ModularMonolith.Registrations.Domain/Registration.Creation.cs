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
    public partial class Registration
    {
        public static Result<Registration> Create(ExternalRegistrationId externalRegistrationId, ExamId examId, OrderId orderId, Candidate candidate, ISystemTimeProvider systemTimeProvider)
        {
            return Result.Create(candidate != null, RegistrationErrors.CandidateCannotBeEmpty.Build())
                .OnSuccess(() => new Registration(externalRegistrationId, examId, orderId, candidate, systemTimeProvider))
                .OnSuccess(registration => registration.RaiseEvent(new RegistrationCreated(registration.Id, registration.ExternalId, candidate, systemTimeProvider.UtcNow)));
        }
    }
}