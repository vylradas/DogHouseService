using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace DogService.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await Handle(context, ex);
            }
        }

        private static Task Handle(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (status, msg) = ex switch
            {
                ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                InvalidOperationException => (HttpStatusCode.Conflict, ex.Message),
                JsonException => (HttpStatusCode.BadRequest, "Invalid JSON."),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            context.Response.StatusCode = (int)status;
            var payload = JsonSerializer.Serialize(new { error = msg });
            return context.Response.WriteAsync(payload);
        }
    }
}
