using System.Net;
using TodoApp.Dtos;
using TodoApp.Exceptions;

namespace TodoApp.Middlewares;

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
        _logger.LogError(exception, "An unexpected error occurred.");

        ExceptionResponse response = exception switch
        {
            InvalidStateTransitionException ex => new ExceptionResponse(HttpStatusCode.BadRequest, ex.Message),
            ResourceAlreadyExistException ex => new ExceptionResponse(HttpStatusCode.Conflict, ex.Message),
            ResourceNotFoundException ex => new ExceptionResponse(HttpStatusCode.NotFound, ex.Message),
            UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
            ActionNotAllowedException ex => new ExceptionResponse(HttpStatusCode.Forbidden, ex.Message),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}