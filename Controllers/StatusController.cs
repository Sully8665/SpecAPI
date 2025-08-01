using Microsoft.AspNetCore.Mvc;

namespace SpecAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(new { status = "Free version running", version = "1.0" });
        }
    }
}
