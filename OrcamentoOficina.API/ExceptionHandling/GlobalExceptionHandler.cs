using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrcamentoOficina.Application.Common.Exceptions;

namespace OrcamentoOficina.API.ExceptionHandlers;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Erro ao processar a requisição. TraceId: {TraceId}", httpContext.TraceIdentifier);

        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,

            ConflictException => StatusCodes.Status409Conflict,

            ConcurrencyException => StatusCodes.Status409Conflict,

            ArgumentException => StatusCodes.Status400BadRequest,

            InvalidOperationException => StatusCodes.Status409Conflict, _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),

            Detail = statusCode == StatusCodes.Status500InternalServerError ? "Ocorreu um erro interno no servidor." : exception.Message,

            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",

            StatusCodes.Status404NotFound => "Not Found",

            StatusCodes.Status409Conflict => "Conflict", 
            
            _ => "Internal Server Error"
        };
    }
}