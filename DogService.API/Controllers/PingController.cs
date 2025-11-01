using Microsoft.AspNetCore.Mvc;

namespace DogService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }
    }
}
