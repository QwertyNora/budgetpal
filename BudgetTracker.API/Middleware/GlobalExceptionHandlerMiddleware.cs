using System.Net;
using System.Text.Json;

namespace BudgetTracker.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var errors = new List<string>();

        switch (exception)
        {
            case ArgumentException argumentException:
                statusCode = HttpStatusCode.BadRequest;
                errors.Add(argumentException.Message);
                break;
            case InvalidOperationException invalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                errors.Add(invalidOperationException.Message);
                break;
            case KeyNotFoundException keyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errors.Add(keyNotFoundException.Message);
                break;
            default:
                errors.Add(exception.Message);
                break;
        }

        var response = new
        {
            status = (int)statusCode,
            message = GetErrorMessage(statusCode),
            errors = errors
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(jsonResponse);
    }

    private static string GetErrorMessage(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest => "Bad request",
            HttpStatusCode.NotFound => "Resource not found",
            HttpStatusCode.InternalServerError => "An internal server error occurred",
            _ => "An error occurred"
        };
    }
}
