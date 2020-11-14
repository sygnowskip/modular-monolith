using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Hexure.API;
using Hexure.Results.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.CommandServices.Exams;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.QueryServices.Exams;
using NSwag.Annotations;

namespace ModularMonolith.API.Controllers.Planning
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

        [HttpGet, Route("{examId}")]
        [SwaggerResponse(typeof(ExamDto))]
        public async Task<IActionResult> Get(long examId)
        {
            return OkOrNotFound(await GetExamQuery.Create(examId)
                .OnSuccess(query => _mediator.Send(query)));
        }

        [HttpGet, Route("/all")]
        [SwaggerResponse(typeof(IEnumerable<ExamDto>))]
        public async Task<IActionResult> GetAll()
        {
            return OkOrNotFound(await GetExamsQuery.Create()
                .OnSuccess(query => _mediator.Send(query)));
        }

        [HttpPost]
        [SwaggerResponse(typeof(void))]
        public async Task<IActionResult> Create(CreateExamRequest request)
        {
            return CreatedOrUnprocessableEntity(await CreateExamCommand.Create(request)
                .OnSuccess(command => _mediator.Send(command)), id => $"/api/exams/{id}");
        }

        [HttpPut, Route("{examId}")]
        [SwaggerResponse(typeof(void))]
        public async Task<IActionResult> Edit(EditExamRequest request)
        {
            return NoContentOrUnprocessableEntity(await EditExamCommand.Create(request)
                .OnSuccess(command => _mediator.Send(command)));
        }

        [HttpDelete, Route("{examId}")]
        [SwaggerResponse(typeof(void))]
        public async Task<IActionResult> Delete(long examId)
        {
            return NoContentOrNotFound(await DeleteExamCommand.Create(examId)
                .OnSuccess(command => _mediator.Send(command)));
        }
    }
}