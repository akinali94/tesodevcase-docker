using OrderService.Configs;
using OrderService.Helpers;

namespace OrderService.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly IKafkaProducerConfig _producer;
    
    public LoggingMiddleware(RequestDelegate next, IKafkaProducerConfig producer, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _producer = producer;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        
        var requestLog = new RequestLog
        {
            Timestamp = DateTime.Now,
            Level = "Info",
            Message = $"HTTP {request.Method} : {request.Path}",
            Source = "Request",
            Host = $"{request.Host}",
            User = $"{context.User.Identity?.Name}"
        };
        
        _logger.LogInformation("Request Details: {requestLog}", requestLog);
        await _producer.ProduceAsync("order-logs", requestLog);
        
        
        
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        //context.Response.Body = responseBody;

        await _next(context);
            
        context.Response.Body = responseBody;

        var result = LogResponse(context);
        
        await _producer.ProduceAsync("order-logs", result);
        await responseBody.CopyToAsync(originalBodyStream);
    }
    
    
    private void LogRequest(HttpContext context)
    {
        var request = context.Request;

        var requestLog = new RequestLog
        {
            Timestamp = DateTime.Now,
            Level = "Info",
            Message = $"HTTP {request.Method} : {request.Path}",
            Source = "Request",
            Host = $"{request.Host}",
            User = $"{context.User.Identity?.Name}"
        };
        
        
        _logger.LogInformation("Request Details: {requestLog}", requestLog);
        _producer.ProduceAsync("order-log", requestLog);
    }

    private ResponseLog LogResponse(HttpContext context)
    {
        var response = context.Response;

        ResponseLog responseLog = new ResponseLog();
        responseLog.Timestamp = DateTime.Now;
        responseLog.Level = "Info";
        if (response.StatusCode >= 500)
            responseLog.Level = "Error";
        string statusCode = response.StatusCode.ToString();
        responseLog.Message = "HTTP" + statusCode;
        responseLog.Source = "response";
        responseLog.ContentType = response.ContentType ?? "unknown";
        
        // var responseLog = new ResponseLog
        // {
        //     Timestamp = DateTime.Now,
        //     Level = "Info",
        //     Message = $"HTTP {response.StatusCode}",
        //     Source = "Response",
        //     ContentType = $"{response.ContentType}",
        //
        // };
        //
        //
        // if (response.StatusCode >= 500)
        //     responseLog.Level = "Error";
            
        _logger.LogInformation($"Response Details: {responseLog}", responseLog);
        return responseLog;
        //await _producer.ProduceAsync("order-logs", responseLog);

    }
}

