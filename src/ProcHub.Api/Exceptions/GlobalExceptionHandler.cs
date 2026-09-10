using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
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
        (int statusCode, string title) = exception switch
        {
            ValidationException => 
                (StatusCodes.Status400BadRequest, 
                    "Validation Failed."),
            
            NotFoundException =>
                (StatusCodes.Status404NotFound, 
                    "Resource not found."),

            DuplicateResourceException =>
                (StatusCodes.Status409Conflict, 
                    "Resource already exists."),
            
            DomainException =>
                (StatusCodes.Status400BadRequest, 
                    "Business rule violation."),
            
            _ =>
                (StatusCodes.Status500InternalServerError,
                    "An unexpected error occured.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(
                exception,
                "Unhandled exception occurred while processing {Method} {Path}",
                httpContext.Request.Method,
                httpContext.Request.Path);      
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
