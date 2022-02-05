using Hexure;
using ModularMonolith.Orders.Language;

namespace ModularMonolith.Orders.Domain
{
    public interface IOrderRepository :  IAggregateRootRepository<Order, OrderId>
    {
        
    }
}