using System;
using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments.Contracts
{
    public interface IPaymentsApplicationService
    {
        Task<Result<PaymentId>> StartPayment(Guid correlationId);
        Task<Result> CompletePayment(PaymentId id);
    }
}