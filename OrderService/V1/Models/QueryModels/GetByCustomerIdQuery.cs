namespace OrderService.V1.Models.QueryModels;

public class GetByCustomerIdQuery
{
    public string CustomerId { get; set; }

    public GetByCustomerIdQuery(string customerId)
    {
        CustomerId = customerId;
    }
    
    public GetByCustomerIdQuery()
    {
    }
}