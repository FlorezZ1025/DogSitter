using DGW.DogSitter.DogWalker.Domain.Exceptions;
using Azure.Core;
using System.Net;

namespace DGW.DogSitter.DogWalker.Api.Middleware;

public class AppExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<AppExceptionHandlerMiddleware> _logger = logger;
    private const string LOG_ERROR = "Se ha presentado un error no controlado {exception.Message} {exception.StackTrace}";
    private const string LOG_INFORMACION = "Se ha presentado un error controlado {exception.Message} {exception.StackTrace}";

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception exception)
        {
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                ErrorMessage = exception.Message
            });

            context.Response.ContentType = ContentType.ApplicationJson.ToString();
            context.Response.StatusCode = exception switch
            {
                NotFoundVoterException => (int)HttpStatusCode.NotFound,
                CoreBusinessException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogInformation(exception, LOG_INFORMACION, exception.Message, exception.StackTrace);
            }
            else
            {
                _logger.LogError(exception, LOG_ERROR, exception.Message, exception.StackTrace);
            }

            await context.Response.WriteAsync(result);
        }
    }
}
