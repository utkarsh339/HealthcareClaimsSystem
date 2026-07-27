using System.Text.Json;
using FluentValidation;
using HealthcareClaims.API.Common;
using HealthcareClaims.Application.Exceptions;

namespace HealthcareClaims.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(
                ex,
                "Unhandled exception occurred while processing {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        ErrorResponse response;

        switch (exception)
        {
            case ValidationException validationException:

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                response = new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Title = "Validation Failed",
                    Message = "One or more validation errors occurred.",
                    TraceId = context.TraceIdentifier,
                    Errors = validationException.Errors
                        .Select(e => e.ErrorMessage)
                };

                break;

            case DuplicateEmailException duplicateEmailException:

                context.Response.StatusCode = StatusCodes.Status409Conflict;

                response = new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Title = "Duplicate Email",
                    Message = duplicateEmailException.Message,
                    TraceId = context.TraceIdentifier
                };

                break;

            case UnauthorizedAccessException unauthorizedException:

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                response = new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Title = "Unauthorized",
                    Message = unauthorizedException.Message,
                    TraceId = context.TraceIdentifier
                };

                break;

            default:

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                response = new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Title = "Server Error",
                    Message = "An unexpected error occurred.",
                    TraceId = context.TraceIdentifier
                };

                break;
        }

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}