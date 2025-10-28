using SAGE.Domain.Common.Exceptions;
using System.Text.Json;

namespace SAGE.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            NotFoundException notFound => (StatusCodes.Status404NotFound, notFound.Message),
            ValidationException validation => (StatusCodes.Status400BadRequest, validation.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Acesso não autorizado."),
            _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.")
        };

        response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            statusCode = (int)statusCode,
            message,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path.Value
        };

        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(json);
    }
}
