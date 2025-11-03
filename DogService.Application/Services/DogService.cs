using DogService.Application.DTOs;
using DogService.Application.Interfaces;
using DogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogService.Application.Services
{
    public class DogService : IDogService
    {
        private readonly IDogRepository _repository;
        // Тут також може бути IMapper, якщо ви його додаєте

        public DogService(IDogRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DogResponseDto>> GetDogsAsync(
            string? attribute,
            string? order,
            int pageNumber,
            int pageSize,
            CancellationToken ct)
        {
            // 1. Валідація параметрів
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            // 2. Виклик репозиторію для отримання Domain Entities
            var dogs = await _repository.GetAllAsync(attribute, order, pageNumber, pageSize, ct);

            // 3. Мапінг Domain Entity -> Response DTO
            return dogs.Select(d => new DogResponseDto
            {
                Name = d.Name,
                Color = d.Color,
                TailLength = d.TailLength, // Мапінг для вимоги API
                Weight = d.Weight            // Мапінг для вимоги API
            }).ToList();
        }

        public async Task<DogResponseDto> CreateDogAsync(DogCreateRequestDto newDogDto, CancellationToken ct)
        {
            // 1. Валідація (з DTO)
            if (newDogDto.TailLength < 0 || newDogDto.Weight < 0)
                throw new ArgumentException("Tail length and weight must be non-negative.");

            if (string.IsNullOrWhiteSpace(newDogDto.Name))
                throw new ArgumentException("Name cannot be empty.");

            // 2. Перевірка на унікальність імені
            var existing = await _repository.GetByNameAsync(newDogDto.Name, ct);
            if (existing != null)
                throw new InvalidOperationException($"Dog with name '{newDogDto.Name}' already exists.");

            // 3. Мапінг DTO -> Domain Entity
            var newDog = new Dog
            {
                Name = newDogDto.Name,
                Color = newDogDto.Color,
                TailLength = newDogDto.TailLength,
                Weight = newDogDto.Weight
            };

            // 4. Збереження
            await _repository.AddAsync(newDog, ct);
            await _repository.SaveChangesAsync(ct);

            // 5. Мапінг Domain Entity -> Response DTO
            return new DogResponseDto
            {
                Name = newDog.Name,
                Color = newDog.Color,
                TailLength = newDog.TailLength,
                Weight = newDog.Weight
            };
        }
    }
}