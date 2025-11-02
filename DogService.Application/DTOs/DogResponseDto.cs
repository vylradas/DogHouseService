namespace DogService.Application.DTOs
{
    public class DogResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        // Використовуємо імена полів, як у вимогах до API
        public int tail_length { get; set; }
        public int weight { get; set; }
    }
}
