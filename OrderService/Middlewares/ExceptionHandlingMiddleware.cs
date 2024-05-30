using System.Net;
using System.Text.Json;
using OrderService.Helpers;

namespace OrderService.Middlewares;

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
            _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
            
            var response = context.Response;
            response.ContentType = "application/json";
            
            switch(ex)
            {
                case CustomException e:
                    //custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    //not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    //unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
   
            var result = JsonSerializer.Serialize(new { message = ex?.Message });
   
            await response.WriteAsync(result);
        }
    }
}