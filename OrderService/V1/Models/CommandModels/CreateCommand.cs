using OrderService.Models;

namespace OrderService.V1.Models.CommandModels;

public class CreateCommand
{
    public CreateCommand(string customerId, int quantity, 
        double price, Address address, List<Product> products)
    {
        CustomerId = customerId;
        Quantity = quantity;
        Price = price;
        Address = address;
        Products = products;
    }
    
    public CreateCommand()
    {
        
    }

    public string CustomerId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Address Address { get; set; }
    public List<Product> Products { get; set; }
}