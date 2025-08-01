using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace SpecAPIFree.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("SpecAPI Free version is running.");
        }
    }
}
