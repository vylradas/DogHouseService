using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DogService.Domain.Entities;

namespace DogService.Application.Interfaces
{
    public interface IDogRepository
    {
        // Primary method used by the service
        Task<IEnumerable<Dog>> GetAllAsync(string? attribute, string? order, int pageNumber, int pageSize, CancellationToken ct);

        // Keep the older name as well so other callers don't break — optional thin wrapper in implementation
        // Task<IEnumerable<Dog>> GetDogsAsync(string? attribute, string? order, int pageNumber, int pageSize, CancellationToken ct);

        Task<Dog?> GetByNameAsync(string name, CancellationToken ct);
        Task AddAsync(Dog dog, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
