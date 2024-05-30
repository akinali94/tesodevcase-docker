namespace OrderService.Helpers;

public class RequestLog
{
    public DateTime Timestamp { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public string Source { get; set; }
    public string Host { get; set; }
    public string User { get; set; }
    
}