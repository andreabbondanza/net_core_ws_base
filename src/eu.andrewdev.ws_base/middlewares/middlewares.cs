using System.Text.Json;
using eu.andrewdev.ws_base.models;

public static class Middlewares
{
    public static async Task ApiKeyCheck(HttpContext context, Func<Task> next)
    {
        if (!context.Request.Headers.ContainsKey("API-KEY"))
        {
            context.Response.StatusCode = 401; // Unauthorized
            context.Response.ContentType = "application/json"; // Imposta il tipo di contenuto su JSON

            var response = new StandardResponse<string>("API-KEY is missing");
            var jsonResponse = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            await context.Response.WriteAsync(jsonResponse);
            return;
        }
        await next.Invoke();
    }
}