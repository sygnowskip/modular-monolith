using System.Threading.Tasks;
using Hexure.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Contracts.Registrations;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.API.Controllers
{
    [Authorize("Registrations")]
    [Route("api/registrations")]
    public class RegistrationsController : MediatorController
    {
        public RegistrationsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreateRegistrationRequest request)
        {
            /*var result = await _registrationApplicationService.Create(request);
            return CreatedOrUnprocessableEntity(result, id => $"/api/registrations/{id}");*/
            return Ok();
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await Mediator.Send(new GetSingleRegistration(new RegistrationId(id)));
            return OkOrNotFound(result);
        }
    }
}