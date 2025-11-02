using DogService.Application.DTOs;
using DogService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace DogService.API.Controllers
{
    [ApiController]
    [Route("dogs")] // Чітко визначимо базовий маршрут
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DogCreateRequestDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateDogAsync(dto, ct);

            // No GetById endpoint currently — return Created with location left null for now
            return CreatedAtAction(nameof(Get), null, created);
        }
    }
}

