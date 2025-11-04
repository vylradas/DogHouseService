using DogService.Application.Interfaces;
using DogService.Application.Services;
using DogService.Infrastructure;
using DogService.Infrastructure.Data;
using DogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// --- Секція Add services to the container ---

// 1. КОНФІГУРАЦІЯ БАЗИ ДАНИХ (DB Context)
builder.Services.AddDbContext<DogsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. РЕЄСТРАЦІЯ DEPENDENCY INJECTION (DI)
// Реєстрація репозиторію (Infrastructure)
builder.Services.AddScoped<IDogRepository, DogRepository>();
// Реєстрація сервісу (Application)
builder.Services.AddScoped<IDogService, DogService.Application.Services.DogService>();

// 3. КОНФІГУРАЦІЯ RATE LIMITER (для API)
// Додаємо RateLimiter, щоб уникнути DDoS-атак або перевантаження
builder.Services.AddRateLimiter(options =>
{
    // Partition by user name (or "anonymous") and create a token-bucket limiter per partition.
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var partitionKey = httpContext.User?.Identity?.Name ?? "anonymous";
        // Use the correct factory method and provide an options factory
        return RateLimitPartition.GetTokenBucketLimiter(partitionKey, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromSeconds(1),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });

    options.RejectionStatusCode = 429;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ENABLE RATE LIMITER middleware (must be called to apply AddRateLimiter configuration)
app.UseRateLimiter();

// Global exception handling middleware (optional but recommended)
app.UseMiddleware<DogService.API.Middleware.ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
