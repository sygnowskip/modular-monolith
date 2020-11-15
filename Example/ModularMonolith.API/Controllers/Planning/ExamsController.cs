using System.Collections.Generic;
using System.Threading.Tasks;
using Hexure.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.CommandServices.Exams;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.Exams.Language;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;
using ModularMonolith.QueryServices.Exams;
using NSwag.Annotations;

namespace ModularMonolith.API.Controllers.Planning
{
    [AllowAnonymous]
    [Route("api/exams")]
    public class ExamsController : MediatorController
    {
        private readonly ISubjectExistenceValidator _subjectExistenceValidator;
        private readonly ILocationExistenceValidator _locationExistenceValidator;

        public ExamsController(IMediator mediator, ISubjectExistenceValidator subjectExistenceValidator,
            ILocationExistenceValidator locationExistenceValidator) : base(mediator)
        {
            _subjectExistenceValidator = subjectExistenceValidator;
            _locationExistenceValidator = locationExistenceValidator;
        }

        [HttpGet, Route("{examId}")]
        [SwaggerResponse(typeof(ExamDto))]
        public Task<IActionResult> Get(long examId)
        {
            return OkOrNotFoundAsync<GetExamQuery, ExamDto>(GetExamQuery.Create(examId));
        }

        [HttpGet, Route("/all")]
        [SwaggerResponse(typeof(IEnumerable<ExamDto>))]
        public Task<IActionResult> GetAll()
        {
            return OkOrUnprocessableEntityAsync<GetExamsQuery, ExamDto>(GetExamsQuery.Create());
        }

        [HttpPost]
        [SwaggerResponse(typeof(void))]
        public Task<IActionResult> Create(CreateExamRequest request)
        {
            return CreatedOrUnprocessableEntityAsync<CreateExamCommand, ExamId>(
                CreateExamCommand.Create(request, _subjectExistenceValidator, _locationExistenceValidator),
                id => $"/api/exams/{id}");
        }

        [HttpPut, Route("{examId}")]
        [SwaggerResponse(typeof(void))]
        public Task<IActionResult> Edit(EditExamRequest request)
        {
            return NoContentOrUnprocessableEntityAsync(EditExamCommand.Create(request));
        }

        [HttpDelete, Route("{examId}")]
        [SwaggerResponse(typeof(void))]
        public Task<IActionResult> Delete(long examId)
        {
            return NoContentOrNotFoundAsync(DeleteExamCommand.Create(examId));
        }
    }
}