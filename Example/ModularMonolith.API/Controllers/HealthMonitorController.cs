using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ModularMonolith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthMonitorController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is working");
        }

        [Authorize("Registrations")]
        [Route("registrations")]
        [HttpGet]
        public IActionResult GetRegistrations()
        {
            return Ok("API is working");
        }

        [Authorize("Payments")]
        [Route("payments")]
        [HttpGet]
        public IActionResult GetPayments()
        {
            return Ok("API is working");
        }
    }
}
