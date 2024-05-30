using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Models;

public class Order
{
    [BsonId]
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Status { get; set; }
    public Address Address { get; set; }
    public List<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}