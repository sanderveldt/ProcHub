using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Api.Exceptions;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        (int statusCode, string title, LogLevel level) = exception switch
        {
            ValidationException => 
                (StatusCodes.Status400BadRequest, 
                    "Validation Failed.",
                    LogLevel.Debug),
            
            NotFoundException =>
                (StatusCodes.Status404NotFound, 
                    "Resource not found.",
                    LogLevel.Information),

            DuplicateResourceException =>
                (StatusCodes.Status409Conflict, 
                    "Resource already exists.",
                    LogLevel.Information),
            
            ResourceInUseException =>
                (StatusCodes.Status409Conflict,
                    "Resource in use.",
                    LogLevel.Information),

            InvalidStateException =>
                (StatusCodes.Status409Conflict,
                    "Resource in an invalid state",
                    LogLevel.Information),
            
            ConcurrencyException => 
                (StatusCodes.Status409Conflict,
                "Concurrency conflict",
                LogLevel.Information),
            
            // EF core exception, fallback for small race condition
            // between application version check and SaveChangesAsync
            // Above Concurrency exception should be thrown for most stale updates
            DbUpdateConcurrencyException =>
                (StatusCodes.Status409Conflict,
                "Concurrency conflict.",
                LogLevel.Information),
            
            DomainException =>
                (StatusCodes.Status400BadRequest, 
                    "Business rule violation.",
                    LogLevel.Information),
            
            ArgumentException =>
                (StatusCodes.Status400BadRequest,
                    "Invalid input.",
                    LogLevel.Information),
            
            _ =>
                (StatusCodes.Status500InternalServerError,
                    "An unexpected error occured.",
                    LogLevel.Error)
        };

        if (level == LogLevel.Error)
        {
            logger.Log(level, exception,
                "Unhandled exception while processing {Method} {Path}",
                httpContext.Request.Method,
                httpContext.Request.Path);
        }
        else
        {
            logger.Log(level,
                "{Method} {Path} returned {StatusCode} ({ExceptionType}): {Message}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                statusCode,
                exception.GetType().Name,
                exception.Message);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,

            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred while processing the request."
                : exception.Message,
            
            Instance = httpContext.Request.Path
        };

        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.ErrorMessage)
                        .ToArray());

            problemDetails.Extensions["errors"] = errors;
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);
        
        return true;
    }
}