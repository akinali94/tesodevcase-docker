namespace OrderService.V1.Models.QueryModels;

public class GetByIdQuery
{
    public string OrderId { get; set; }

    public GetByIdQuery(string orderId)
    {
        OrderId = orderId;
    }
    
    public GetByIdQuery()
    {

    }
}