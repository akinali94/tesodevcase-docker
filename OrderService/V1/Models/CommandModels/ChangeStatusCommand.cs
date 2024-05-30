namespace OrderService.V1.Models.CommandModels;

public class ChangeStatusCommand
{
    public string OrderId { get; set; }
    public string NewStatus { get; set; }

    public ChangeStatusCommand()
    {
        
    }
    public ChangeStatusCommand(string orderId, string newStatus)
    {
        OrderId = orderId;
        NewStatus = newStatus;
    }
    
}