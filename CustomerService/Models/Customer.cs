using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models;

public class Customer
{
    [BsonId]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Address> Addresses { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}