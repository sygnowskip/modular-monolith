using System.Threading.Tasks;
using Hexure.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.CommandServices;

namespace ModularMonolith.API.Controllers
{
    [AllowAnonymous]
    [Route("api/exams")]
    public class ExamsController : RestfulController
    {
        private readonly IMediator _mediator;

        public ExamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _mediator.Send(new CreateExamCommand());
            
            return Ok();
        }
    }
}