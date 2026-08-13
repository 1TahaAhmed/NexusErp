using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NexusErp.Application.Common;
using Nexus.Erp.Domain.ExceptionHandling;
using Nexus.Erp.Domain.ExeptionHandling;
using NexusErp.Application.Common.Responses;

namespace NexusErp.API.Middlewares;

public  class ExceptionHandlingMiddleware
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

        var response = exception switch
        {
            NotFoundException => new
            {
                StatusCode = HttpStatusCode.NotFound,
                Body = ApiResponse<object>.Failure(exception.Message)
            },
            BadRequestException ex => new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Body = ApiResponse<object>.Failure(ex.Message)
            },
            ValidationException ex => new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Body = ApiResponse<object>.Failure(ex.Message, ex.Errors)
            },
            DbUpdateException => new
            {
                StatusCode = HttpStatusCode.Conflict,
                Body = ApiResponse<object>.Failure("A database update error occurred.")
            },
            _ => new
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Body = ApiResponse<object>.Failure("An unexpected error occurred.")
            }
        };
        context.Response.StatusCode = (int)response.StatusCode;
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response.Body, jsonOptions));
    }
}