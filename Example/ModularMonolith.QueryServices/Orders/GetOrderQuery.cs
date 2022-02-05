using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hexure.Results;
using MediatR;
using ModularMonolith.Contracts.Orders;
using ModularMonolith.Errors;

namespace ModularMonolith.QueryServices.Orders
{
    public class GetOrderQuery : IRequest<Result<OrderDto>>
    {
        public GetOrderQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }

        public static Result<GetOrderQuery> Create(long orderId)
        {
            return Result.Ok(new GetOrderQuery(orderId));
        }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<OrderDto>>
    {
        private readonly IDbConnection _dbConnection;

        public GetOrderQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var orders = new Dictionary<long, OrderDto>();
            await _dbConnection.QueryAsync<OrderDto, OrderItemDto, OrderDto>(BuildQuery(), (order, item) =>
            {
                if (!orders.ContainsKey(order.Id))
                {
                    order.Items ??= new List<OrderItemDto>();
                    orders.Add(order.Id, order);
                }

                orders[order.Id].Items.Add(item);
                return order;
            }, new { id = request.Id });

            return orders.Count() == 1
                ? Result.Ok(orders.Values.Single())
                : Result.Fail<OrderDto>(DomainErrors.BuildNotFound("Order", request.Id));
        }

        private string BuildQuery()
        {
            return @"
SELECT o.Id, o.Status, o.CreationDateTime, oi.Id, oi.ExternalId, oi.Name, oi.ProductType
FROM [orders].[Order] o
JOIN [orders].[OrderItem] oi ON oi.OrderId = o.Id
WHERE o.Id = @id";
        }
    }
}