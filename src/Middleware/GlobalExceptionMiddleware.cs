using System.Net;
using System.Text.Json;

namespace EasyFood.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteErrorResponse(context, ex);
        }
    }

    private static Task WriteErrorResponse(HttpContext context, Exception ex)
    {
        var (statusCode, title) = ex switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, "Recurso não encontrado"),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden, "Acesso negado"),
            ArgumentException => (HttpStatusCode.BadRequest, "Requisição inválida"),
            _ => (HttpStatusCode.InternalServerError, "Erro interno do servidor")
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new
        {
            type = $"https://httpstatuses.com/{(int)statusCode}",
            title,
            status = (int)statusCode,
            detail = ex.Message,
            traceId = context.TraceIdentifier
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
