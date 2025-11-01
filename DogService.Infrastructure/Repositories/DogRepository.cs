using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogService.Application.Interfaces;
using DogService.Domain.Entities;
using DogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DogService.Infrastructure.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly DogsDbContext _db;
        public DogRepository(DogsDbContext db) => _db = db;

        public async Task<IEnumerable<Dog>> GetDogsAsync(string? attribute, string? order, int pageNumber, int pageSize, CancellationToken ct)
        {
            IQueryable<Dog> q = _db.Dogs.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(attribute))
            {
                var asc = string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase);
                var desc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
                q = attribute.ToLower() switch
                {
                    "weight" => desc ? q.OrderByDescending(d => d.Weight) : q.OrderBy(d => d.Weight),
                    "tail_length" => desc ? q.OrderByDescending(d => d.TailLength) : q.OrderBy(d => d.TailLength),
                    "name" => desc ? q.OrderByDescending(d => d.Name) : q.OrderBy(d => d.Name),
                    "color" => desc ? q.OrderByDescending(d => d.Color) : q.OrderBy(d => d.Color),
                    _ => q
                };
            }

            q = q.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return await q.ToListAsync(ct);
        }

        public async Task<Dog?> GetByNameAsync(string name, CancellationToken ct) =>
            await _db.Dogs.FirstOrDefaultAsync(d => d.Name == name, ct);

        public async Task AddAsync(Dog dog, CancellationToken ct)
        {
            await _db.Dogs.AddAsync(dog, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct) => await _db.Dogs.CountAsync(ct);
    }
}
