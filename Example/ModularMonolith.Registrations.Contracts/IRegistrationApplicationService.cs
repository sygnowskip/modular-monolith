using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Contracts
{
    public interface IRegistrationApplicationService
    {
        Task<Result> StartPaymentAsync(RegistrationId id);
        Task<Result> MarkAsPaid(RegistrationId id);
    }
}