using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogService.Domain.Entities;

namespace DogService.Application.Interfaces
{
    public interface IDogRepository
    {
        Task<IEnumerable<Dog>> GetDogsAsync(string? attribute, string? order, int pageNumber, int pageSize, CancellationToken ct);
        Task<Dog?> GetByNameAsync(string name, CancellationToken ct);
        Task AddAsync(Dog dog, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
    }
}
