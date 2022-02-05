using System.Threading.Tasks;
using Hexure.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Contracts.Orders;
using ModularMonolith.QueryServices.Orders;
using NSwag.Annotations;

namespace ModularMonolith.API.Controllers.Orders
{
    [Authorize]
    [Route("api/orders")]
    public class OrdersController : MediatorController
    {
        public OrdersController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet, Route("{orderId}")]
        [SwaggerResponse(typeof(OrderDto))]
        public Task<IActionResult> Get(long orderId)
        {
            return OkOrNotFoundAsync<GetOrderQuery, OrderDto>(GetOrderQuery.Create(orderId));
        }
    }
}