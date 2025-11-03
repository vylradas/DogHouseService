using DogService.Application.DTOs; 
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DogService.Application.Interfaces
{
    public interface IDogService
    {
        // Повертає DTO для API
        Task<IEnumerable<DogResponseDto>> GetDogsAsync(
            string? attribute,
            string? order,
            int pageNumber,
            int pageSize,
            CancellationToken ct);

        // Приймає DTO та повертає DTO
        Task<DogResponseDto> CreateDogAsync(DogCreateRequestDto newDogDto, CancellationToken ct);
        // Task<DogResponseDto> CreateDogAsync(DogCreateRequestDto request, CancellationToken ct);
    }
}


