using System.Threading.Tasks;
using Hexure.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.QueryServices;

namespace ModularMonolith.API.Controllers.Common
{
    [Authorize]
    [Route("api/locations")]
    public class LocationsController : RestfulController
    {
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetLocationsQuery()));
        }
    }
}