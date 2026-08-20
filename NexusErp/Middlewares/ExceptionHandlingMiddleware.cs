using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NexusErp.Application.Common;
using Nexus.Erp.Domain.ExceptionHandling;
using Nexus.Erp.Domain.ExeptionHandling;
using NexusErp.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace NexusErp.API.Middlewares;

public sealed class ExceptionHandlingMiddleware
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
            _logger.LogError(ex, "An unhandled exception occurred: {Message}.", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    public static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";


        var (statusCode, body) = exception switch
        {
            NotFoundException => (
                HttpStatusCode.NotFound,
                ApiResponse<object>.Failure(exception.Message)
            ),
            BadRequestException ex => (
                HttpStatusCode.BadRequest,
                ApiResponse<object>.Failure(ex.Message)
            ),
            ValidationException ex => (
                HttpStatusCode.BadRequest,
                ApiResponse<object>.Failure(ex.Message, ex.Errors)
            ),
            DbUpdateException => (
                HttpStatusCode.Conflict,
                ApiResponse<object>.Failure("A database update error occurred.")
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                ApiResponse<object>.Failure("An unexpected error occurred.")
            )
        };

        context.Response.StatusCode = (int)statusCode;
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(body, jsonOptions));
    }
}