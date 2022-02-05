using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Orders.Domain;
using ModularMonolith.Orders.Language;

namespace ModularMonolith.Orders.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IOrdersDbContext _ordersDbContext;

        public OrderRepository(IOrdersDbContext ordersDbContext)
        {
            _ordersDbContext = ordersDbContext;
        }

        public async Task<Result<Order>> SaveAsync(Order aggregate)
        {
            await _ordersDbContext.Orders.AddAsync(aggregate);
            await _ordersDbContext.SaveChangesAsync();
            return Result.Ok(aggregate);
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