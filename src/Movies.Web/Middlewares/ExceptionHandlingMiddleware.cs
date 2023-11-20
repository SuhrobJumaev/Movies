
using FluentValidation;
using Movies.BusinessLogic;
using System.Net;

namespace Movies.Web;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
   private readonly ILogger<ExceptionHandlingMiddleware> _log;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> log)
    {
        _next = next;
        _log = log;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            
            await _next.Invoke(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(httpContext, ex, httpContext.RequestAborted);
        }
        catch (Exception ex) 
        {
            await HandleExceptionAsync(httpContext, ex, httpContext.RequestAborted); 
        }
    }

    private Task HandleValidationExceptionAsync(HttpContext httpContext, ValidationException ex, CancellationToken token = default)
    {
        var errorResponse = new ValidationFailureResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = "Request validation error!",
            Errors = ex.Errors.Select(x => new ValidationResponse
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage
            })
        };

        httpContext.Response.ContentType = Utils.JsonContentType;
        httpContext.Response.StatusCode = (int)errorResponse.StatusCode;

        return httpContext.Response.WriteAsJsonAsync(errorResponse, token); 
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception ex, CancellationToken token = default)
    {
        _log.LogError(ex, "Failed to execute the request!");

        var errorResponse = ex switch
        {
            _ => new { 
                StatusCode = HttpStatusCode.InternalServerError, 
                Message = "Failed to execute the request, error: " + ex.Message, 
                Errors = Enumerable.Empty<string>() }
        };

        httpContext.Response.ContentType = Utils.JsonContentType;
        httpContext.Response.StatusCode = (int)errorResponse.StatusCode;

        return httpContext.Response.WriteAsJsonAsync(errorResponse, token); 
    }
}