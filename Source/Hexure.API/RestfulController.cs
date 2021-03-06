﻿using System;
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

        //GET
        protected IActionResult OkOrNotFound<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound();
        }

        //GET MULTIPLE
        protected IActionResult OkOrUnprocessableEntity<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return UnprocessableEntity(result.Error);
        }

        //DELETE, PUT
        protected IActionResult NoContentOrUnprocessableEntityOrNotFound(Result result, Error.ErrorType notFoundErrorType)
        {
            if (result.IsSuccess)
            {
                return NoContent();
            }

            if (result.ViolatesOnly(notFoundErrorType))
            {
                return NotFound();
            }

            return UnprocessableEntity(result.Error);
        }
    }
}