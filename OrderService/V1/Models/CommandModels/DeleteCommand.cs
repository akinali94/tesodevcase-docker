namespace OrderService.V1.Models.CommandModels;

public class DeleteCommand
{
    public string OrderId { get; set; }

    public DeleteCommand(string orderId)
    {
        OrderId = orderId;
    }
    
    public DeleteCommand()
    {
    }
}