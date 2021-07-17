using System.Threading.Tasks;
using Hexure.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.QueryServices.Common;

namespace ModularMonolith.API.Controllers.Common
{
    [Authorize]
    [Route("api/locations")]
    public class LocationsController : MediatorController
    {
        public LocationsController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetLocationsQuery()));
        }
    }
}