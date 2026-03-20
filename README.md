DogHouseService — це приклад REST API, розроблений за допомогою ASP.NET Core Web API і Entity Framework Core (Code First).
Сервіс надає доступ до інформації про собак, дозволяє виконувати сортування, пагінацію та додавання нових записів у базу даних.

Технології
.NET 8 / ASP.NET Core Web API
Entity Framework Core
SQLite (локальна база даних)
xUnit, Moq (юнiт-тестування)

## Структура проєкту
DogService/
├── DogService.API/             # REST API (Controllers)
├── DogService.Application/     # Бізнес-логіка, DTOs, сервіси
├── DogService.Domain/          # Сутності домену (Dog)
├── DogService.Infrastructure/  # Репозиторії, DbContext
└── DogService.Tests/           # Unit-тести
