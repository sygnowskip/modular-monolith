using System.Threading.Tasks;
using Hexure.API;
using Hexure.Time;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.CommandServices.Registrations;
using ModularMonolith.Contracts.Registrations;
using ModularMonolith.Exams.Language.Validators;
using ModularMonolith.QueryServices.Registrations;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.API.Controllers
{
    [Authorize("Registrations")]
    [Route("api/registrations")]
    public class RegistrationsController : MediatorController
    {
        private readonly IExamExistenceValidator _examExistenceValidator;
        private readonly ISystemTimeProvider _systemTimeProvider;

        public RegistrationsController(IMediator mediator, IExamExistenceValidator examExistenceValidator,
            ISystemTimeProvider systemTimeProvider) : base(mediator)
        {
            _examExistenceValidator = examExistenceValidator;
            _systemTimeProvider = systemTimeProvider;
        }

        [HttpPost]
        public Task<IActionResult> Save(CreateRegistrationRequest request)
        {
            return CreatedOrUnprocessableEntityAsync<CreateRegistrationCommand, RegistrationId>(
                CreateRegistrationCommand.Create(request, _systemTimeProvider, _examExistenceValidator), 
                id => $"/api/registrations/{id}");
        }

        [HttpGet, Route("{registrationId}")]
        public Task<IActionResult> Get(long registrationId)
        {
            return OkOrNotFoundAsync<GetRegistrationQuery, RegistrationDto>(GetRegistrationQuery.Create(registrationId));
        }
    }
}