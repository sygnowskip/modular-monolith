using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Registrations.Contracts;
using ModularMonolith.Registrations.Contracts.Queries;
using ModularMonolith.Registrations.Contracts.Requests;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.API.Controllers
{
    //[Authorize("Registrations")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationApplicationService _registrationApplicationService;
        private readonly IMediator _mediator;

        public RegistrationsController(IRegistrationApplicationService registrationApplicationService, IMediator mediator)
        {
            _registrationApplicationService = registrationApplicationService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Save(RegistrationCreationRequest request)
        {
            var result = await _registrationApplicationService.Create(request);

            //TODO: Validation errors standard
            //TODO: Extensions to cast Result to Response (200 / 422)
            //TODO: Id should be in Headers.Location with 201 HTTP
            return result.IsSuccess ? Ok(result.Value) : UnprocessableEntity() as IActionResult;
        }

        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetSingleRegistration(RegistrationId.CreateFor(id)));

            return result.IsSuccess ? Ok(result.Value) : NotFound() as IActionResult;
        }
    }
}