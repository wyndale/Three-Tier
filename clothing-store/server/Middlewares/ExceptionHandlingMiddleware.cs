using server.DTOs;
using server.Exceptions;
using System.Net;
using System.Text.Json;

namespace server.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionHandlingMiddleware(RequestDelegate next,
                                       ILogger<ExceptionHandlingMiddleware> logger,
                                       IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    // Centralized exception handling middleware
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Proceed to the next middleware/component
            await _next(context);
        }
        catch (Exception ex)
        {
            // Handle exceptions centrally
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log exception details for debugging
        _logger.LogError(exception, "An unhandled exception has occurred. Request Path: {RequestPath}, User: {User}",
            context.Request.Path, context.User.Identity?.Name);

        // Prepare the response details
        var response = context.Response;
        response.ContentType = "application/json";

        // Map exception to HTTP status code and error response
        var errorResponse = new ErrorResponse
        {
            Success = false,
            Message = exception.Message,
            Details = _environment.IsDevelopment() ? exception.InnerException?.Message : null // Only expose inner exception in development
        };

        // Map exceptions to HTTP status codes
        response.StatusCode = exception switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            CategoryAlreadyExistException => (int)HttpStatusCode.Conflict,
            CategoryNotFoundException => (int)HttpStatusCode.NotFound,
            NoProductsAvailableException => (int)HttpStatusCode.NotFound,
            InvalidProductException => (int)HttpStatusCode.BadRequest,
            ProductNotFoundException => (int)HttpStatusCode.NotFound,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError // Default to internal server error
        };


        // Serialize the error response and write it to the response body
        var jsonResponse = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(jsonResponse);
    }
}
