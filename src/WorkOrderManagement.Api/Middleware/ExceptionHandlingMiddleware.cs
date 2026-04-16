using System.Net;
using System.Text.Json;
using WorkOrderManagement.Api.Contracts.Common;
using WorkOrderManagement.Application.Common.Exceptions;

namespace WorkOrderManagement.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await WriteErrorResponseAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message,
                ex.Errors);
        }
        catch (NotFoundException ex)
        {
            await WriteErrorResponseAsync(
                context,
                HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorResponseAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorResponseAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (Exception)
        {
            await WriteErrorResponseAsync(
                context,
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message,
        IReadOnlyList<string>? errors = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,
            Errors = errors
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}