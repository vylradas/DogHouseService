using System.Text.Json.Serialization;

namespace DogService.Application.DTOs
{
    public class DogResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        // Використовуємо імена полів, як у вимогах до API
        [JsonPropertyName("tail_length")]
        public int TailLength { get; set; } // помилка через зміну назви 
        [JsonPropertyName("weight")]
        public int Weight { get; set; } // помилка через зміну назви
    }
}
