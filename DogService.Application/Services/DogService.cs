using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogService.Application.Interfaces;
using DogService.Domain.Entities;

namespace DogService.Application.Services
{
    public class DogService
    {
        private readonly IDogRepository _repo;

        public DogService(IDogRepository repo) => _repo = repo;

        public async Task<IEnumerable<Dog>> GetDogsAsync(string? attribute, string? order, int pageNumber, int pageSize, CancellationToken ct)
        {
            // basic validation for paging
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            return await _repo.GetDogsAsync(attribute, order, pageNumber, pageSize, ct);
        }

        public async Task<Dog> CreateDogAsync(Dog dog, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dog.Name))
                throw new ArgumentException("Name is required");

            if (dog.TailLength < 0)
                throw new ArgumentException("Tail length must be non-negative");

            var exists = await _repo.GetByNameAsync(dog.Name, ct);
            if (exists != null)
                throw new InvalidOperationException("Dog with the same name already exists");

            await _repo.AddAsync(dog, ct);
            return dog;
        }
    }
}

