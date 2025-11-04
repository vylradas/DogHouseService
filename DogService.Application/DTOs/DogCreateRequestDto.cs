using System.Text.Json.Serialization;

namespace DogService.Application.DTOs
{
    public class DogCreateRequestDto
    {
        // Не використовуємо Id, оскільки він створюється в БД
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        [JsonPropertyName("tail_length")]
        public int TailLength { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }
    }
}
