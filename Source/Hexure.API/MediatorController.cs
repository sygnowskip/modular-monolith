using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hexure.API
{
    public class MediatorController : RestfulController
    {
        protected readonly IMediator Mediator;

        public MediatorController(IMediator mediator)
        {
            Mediator = mediator;
        }

        //POST
        protected async Task<IActionResult> CreatedOrUnprocessableEntityAsync<TCommand, TResult>(Result<TCommand> commandFactoryResult, Func<TResult, string> locationUrlFactory)
            where TCommand : IRequest<Result<TResult>>
        {
            var result = await commandFactoryResult
                .OnSuccess(async query => await Mediator.Send(query));

            return CreatedOrUnprocessableEntity(result, locationUrlFactory);
        }

        //GET
        protected async Task<IActionResult> OkOrNotFoundAsync<TQuery, TResponse>(Result<TQuery> queryFactoryResult)
        where TQuery : IRequest<Result<TResponse>>
        {
            var result = await queryFactoryResult
                .OnSuccess(async query => await Mediator.Send(query));

            return OkOrNotFound(result);
        }

        //GET MULTIPLE
        protected async Task<IActionResult> OkOrUnprocessableEntityAsync<TQuery, TResponse>(Result<TQuery> queryFactoryResult)
        where TQuery : IRequest<IEnumerable<TResponse>>
        {
            var result = await queryFactoryResult
                .OnSuccess(async query => await Mediator.Send(query));

            return OkOrUnprocessableEntity(result);
        }

        //DELETE, PUT
        protected async Task<IActionResult> NoContentOrUnprocessableEntityOrNotFound<TCommand>(Result<TCommand> commandFactoryResult, Error.ErrorType notFoundErrorType)
            where TCommand : IRequest<Result>
        {
            var result = await commandFactoryResult
                .OnSuccess(async query => await Mediator.Send(query));

            return NoContentOrUnprocessableEntityOrNotFound(result, notFoundErrorType);
        }
    }
}