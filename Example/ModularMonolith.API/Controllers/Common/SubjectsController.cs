using System.Threading.Tasks;
using Hexure.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.QueryServices;

namespace ModularMonolith.API.Controllers.Common
{
    [Authorize]
    [Route("api/subjects")]
    public class SubjectsController : MediatorController
    {
        public SubjectsController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetSubjectsQuery()));
        }
    }
}