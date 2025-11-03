using DogService.Application.Interfaces;
using DogService.Domain.Entities;
using DogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DogService.Infrastructure.Repositories
{
    public class DogRepository : IDogRepository
    {
        // ВИПРАВЛЕНО: Використовуємо DogsDbContext згідно з останньою конфігурацією
        private readonly DogsDbContext _context;

        public DogRepository(DogsDbContext context) => _context = context;

        public async Task<IEnumerable<Dog>> GetAllAsync(
            string? attribute,
            string? order,
            int pageNumber,
            int pageSize,
            CancellationToken ct)
        {
            IQueryable<Dog> query = _context.Dogs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(attribute))
            {
                // Стиль Варіанту 2
                var desc = string.Equals(order, "desc", System.StringComparison.OrdinalIgnoreCase);
                query = attribute.ToLower() switch
                {
                    "name" => desc ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                    "color" => desc ? query.OrderByDescending(d => d.Color) : query.OrderBy(d => d.Color),
                    // Важливо: використовувати імена властивостей Dog, а не JSON-поля
                    "tail_length" => desc ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength),
                    "weight" => desc ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight),
                    _ => query
                };
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync(ct);
        }

        public async Task<Dog?> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Dogs.FirstOrDefaultAsync(d => d.Name == name, ct);
        }

        public async Task AddAsync(Dog dog, CancellationToken ct)
        {
            await _context.Dogs.AddAsync(dog, ct);
            // НЕ ВИКЛИКАЄМО SaveChangesAsync тут! Це робить DogService.
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            // Метод, контрольований сервісом для Unit of Work
            await _context.SaveChangesAsync(ct);
        }
    }
}