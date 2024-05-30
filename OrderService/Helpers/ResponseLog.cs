namespace OrderService.Helpers;

public class ResponseLog
{
    public DateTime Timestamp { get; set; }
    public string? Level { get; set; }
    public string? Message { get; set; }
    public string? Source { get; set; }
    public string? ContentType { get; set; }
}