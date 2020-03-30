using System;
using Hexure.Results;
using Microsoft.AspNetCore.Mvc;

namespace Hexure.API
{
    [ApiController]
    public abstract class RestfulController : ControllerBase
    {
        //POST
        public IActionResult CreatedOrUnprocessableEntity<T>(Result<T> result, Func<T, string> locationUrlFactory)
        {
            if (result.IsSuccess)
            {
                return Created(locationUrlFactory(result.Value), null);
            }

            return UnprocessableEntity(result.Error);
        }

        //PUT
        public IActionResult OkOrUnprocessableEntity(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok();
            }

            return UnprocessableEntity(result.Error);
        }

        //GET
        public IActionResult OkOrNotFound<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound();
        }

        //DELETE
        public IActionResult NoContentOrNotFound(Result result)
        {
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}