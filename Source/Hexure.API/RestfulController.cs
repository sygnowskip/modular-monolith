using System;
using Hexure.Results;
using Microsoft.AspNetCore.Mvc;

namespace Hexure.API
{
    [ApiController]
    public abstract class RestfulController : ControllerBase
    {
        //POST
        protected IActionResult CreatedOrUnprocessableEntity<T>(Result<T> result, Func<T, string> locationUrlFactory)
        {
            if (result.IsSuccess)
            {
                return Created(locationUrlFactory(result.Value), null);
            }

            return UnprocessableEntity(result.Error);
        }

        //PUT
        protected IActionResult NoContentOrUnprocessableEntity(Result result)
        {
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return UnprocessableEntity(result.Error);
        }

        //GET
        protected IActionResult OkOrNotFound<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound();
        }

        //DELETE
        protected IActionResult NoContentOrNotFound(Result result)
        {
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}