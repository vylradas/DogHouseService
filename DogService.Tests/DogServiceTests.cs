using DogService.Application.DTOs;
using DogService.Application.Interfaces;
using DogService.Domain.Entities;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DogService.Tests
{
    public class DogServiceTests
    {
        // Тест 1: Перевірка успішного створення собаки
        [Fact]
        public async Task CreateDog_Succeeds()
        {
            // ARRANGE (Налаштування)
            var repo = new Mock<IDogRepository>();

            // Налаштовуємо репозиторій: GetByNameAsync має повернути null (собаки не існує)
            repo.Setup(r => r.GetByNameAsync("Doggy", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Dog?)null);

            // Налаштовуємо репозиторій: AddAsync має просто завершитися
            repo.Setup(r => r.AddAsync(It.IsAny<Dog>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Налаштовуємо SaveChangesAsync, оскільки сервіс викликає його після AddAsync
            repo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Створюємо екземпляр сервісу, передаючи йому імітований репозиторій
            var service = new DogService.Application.Services.DogService(repo.Object);
            var dto = new DogCreateRequestDto { Name = "Doggy", Color = "red", TailLength = 5, Weight = 10 };

            // ACT (Дія)
            var result = await service.CreateDogAsync(dto, CancellationToken.None);

            // ASSERT (Перевірка)
            // 1. Перевіряємо, що повернутий об'єкт має коректне ім'я
            Assert.Equal("Doggy", result.Name);

            // 2. Перевіряємо, що метод AddAsync був викликаний рівно один раз
            repo.Verify(r => r.AddAsync(It.IsAny<Dog>(), It.IsAny<CancellationToken>()), Times.Once);

            // 3. Перевіряємо, що SaveChangesAsync був викликаний
            repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // Тест 2: Перевірка викидання винятку при спробі створити дублікат імені
        [Fact]
        public async Task CreateDog_Throws_WhenDuplicate()
        {
            // ARRANGE (Налаштування)
            var repo = new Mock<IDogRepository>();

            // Налаштовуємо репозиторій: GetByNameAsync має повернути існуючу собаку
            repo.Setup(r => r.GetByNameAsync("Doggy", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dog { Name = "Doggy" });

            // Налаштовуємо SaveChangesAsync щоб уникнути помилок, якщо код викличе його помилково
            repo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new DogService.Application.Services.DogService(repo.Object);
            var dto = new DogCreateRequestDto { Name = "Doggy", Color = "red", TailLength = 5, Weight = 10 };

            // ACT & ASSERT (Дія та Перевірка)
            // Очікуємо, що виклик методу Throw (викине) InvalidOperationException
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CreateDogAsync(dto, CancellationToken.None));

            // Додаткова перевірка: переконуємося, що AddAsync НЕ був викликаний
            repo.Verify(r => r.AddAsync(It.IsAny<Dog>(), It.IsAny<CancellationToken>()), Times.Never);

            // І SaveChangesAsync також НЕ був викликаний
            repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}