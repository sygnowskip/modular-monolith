using Hexure.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ModularMonolith.API.Controllers
{
    [Route("api/health-monitor")]
    public class HealthMonitorController : RestfulController
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is working");
        }
    }
}
