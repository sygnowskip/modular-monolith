using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Payments;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Persistence.Repositories
{
    internal class PaymentRepository : IPaymentRepository
    {
        public Task<Result<Payment>> SaveAsync(Payment aggregate)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<Payment>> GetAsync(PaymentId identifier)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> Delete(Payment aggregate)
        {
            throw new System.NotImplementedException();
        }
    }
}