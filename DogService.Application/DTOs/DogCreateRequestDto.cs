namespace DogService.Application.DTOs
{
    public class DogCreateRequestDto
    {
        // Не використовуємо Id, оскільки він створюється в БД
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int TailLength { get; set; }
        public int Weight { get; set; }
    }
}
