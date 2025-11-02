using Microsoft.AspNetCore.Mvc;

namespace DogService.API.Controllers
{
    [ApiController]
    [Route("ping")] // Фіксований маршрут, як у завданні
    public class PingController : ControllerBase
    {
        // Необов'язково, але часто використовується для конфігурації
        private const string ServiceVersion = "Dogshouseservice.Version1.0.1";

        [HttpGet]
        // 'Ping' як назву методу, а не загальний 'Get' для кращої семантики
        public IActionResult Ping() => Ok(ServiceVersion);
    }
}
