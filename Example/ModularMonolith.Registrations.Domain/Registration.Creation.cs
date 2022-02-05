using System.Threading.Tasks;
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
        public static async Task<Result<Registration>> CreateAsync(ExternalRegistrationId externalRegistrationId,
            ExamId examId, OrderId orderId, Candidate candidate, ISystemTimeProvider systemTimeProvider,
            IRegistrationRepository registrationRepository)
        {
            return await Result.Create(candidate != null, RegistrationErrors.CandidateCannotBeEmpty.Build())
                .OnSuccess(() =>
                    new Registration(externalRegistrationId, examId, orderId, candidate, systemTimeProvider))
                .OnSuccess(async registration => await registrationRepository.SaveAsync(registration))
                .OnSuccess(registration => registration.RaiseEvent(new RegistrationCreated(registration.Id,
                    registration.ExternalId, candidate, systemTimeProvider.UtcNow)));
        }
    }
}