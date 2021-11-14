using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Orders.Domain;
using ModularMonolith.Orders.Language;

namespace ModularMonolith.Orders.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Result<Order>> SaveAsync(Order aggregate)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<Order>> GetAsync(OrderId identifier)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> Delete(Order aggregate)
        {
            throw new System.NotImplementedException();
        }
    }
}