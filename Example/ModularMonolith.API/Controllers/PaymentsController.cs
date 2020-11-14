using Hexure.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ModularMonolith.API.Controllers
{
    [Authorize("Payments")]
    [Route("api/payments")]
    public class PaymentsController : RestfulController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}