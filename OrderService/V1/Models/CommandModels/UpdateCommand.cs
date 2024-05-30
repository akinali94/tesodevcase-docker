using OrderService.Models;

namespace OrderService.V1.Models.CommandModels;

public class UpdateCommand
{
    public UpdateCommand(string id, string customerId, int quantity, Address addresses, List<Product> products)
    {
        Id = id;
        CustomerId = customerId;
        Quantity = quantity;
        Addresses = addresses;
        Products = products;
    }
    
    public UpdateCommand()
    {
    }
    
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public int Quantity { get; set; }
    public Address Addresses { get; set; }
    public List<Product> Products { get; set; }


}