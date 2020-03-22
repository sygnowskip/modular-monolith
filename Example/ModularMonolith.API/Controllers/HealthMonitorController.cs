using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ModularMonolith.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class HealthMonitorController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is working");
        }
    }
}
