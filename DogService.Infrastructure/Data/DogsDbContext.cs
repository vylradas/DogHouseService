using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogService.Infrastructure.Data
{
    public class DogsDbContext : DbContext
    {
        public DogsDbContext(DbContextOptions<DogsDbContext> options) : base(options) { }
        public DbSet<Dog> Dogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Dog>().HasIndex(d => d.Name).IsUnique();
            // Seed example rows matching task (Neo, Jessy)
            builder.Entity<Dog>().HasData(
                new Dog { Id = 1, Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
                new Dog { Id = 2, Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
            );
        }
    }
}
