using DogService.Application.DTOs;
using DogService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace DogService.API.Controllers
{
    [ApiController]
    [Route("dogs")] // collection route
    public class DogsController : ControllerBase
    {
        private readonly IDogService _service;

        public DogsController(IDogService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? attribute, [FromQuery] string? order, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            var dogs = await _service.GetDogsAsync(attribute, order, pageNumber, pageSize, ct);
            return Ok(dogs);
        }

        // Keep POST at /dogs
        [HttpPost]
        // Also allow POST at /dog (task requires /dog)
        [HttpPost("/dog")]
        public async Task<IActionResult> Create([FromBody] DogCreateRequestDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateDogAsync(dto, ct);

            return CreatedAtAction(nameof(Get), null, created);
        }
    }
}

