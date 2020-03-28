using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Registrations.Contracts.Requests;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Contracts
{
    public interface IRegistrationApplicationService
    {
        Task<Result<RegistrationId>> Create(RegistrationCreationRequest request);
        Task<Result> StartPaymentAsync(RegistrationId id);
        Task<Result> MarkAsPaid(RegistrationId id);
    }
}