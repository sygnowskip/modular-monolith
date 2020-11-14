using System;
using System.Threading.Tasks;
using Hexure.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Registrations.Contracts;
using ModularMonolith.Registrations.Contracts.Queries;
using ModularMonolith.Registrations.Contracts.Requests;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.API.Controllers
{
    [Authorize("Registrations")]
    [Route("api/registrations")]
    public class RegistrationsController : MediatorController
    {
        private readonly IRegistrationApplicationService _registrationApplicationService;

        public RegistrationsController(IMediator mediator,
            IRegistrationApplicationService registrationApplicationService) : base(mediator)
        {
            _registrationApplicationService = registrationApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(RegistrationCreationRequest request)
        {
            var result = await _registrationApplicationService.Create(request);
            return CreatedOrUnprocessableEntity(result, id => $"/api/registrations/{id}");
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetSingleRegistration(new RegistrationId(id)));
            return OkOrNotFound(result);
        }
    }
}