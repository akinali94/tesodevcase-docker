using MongoDB.Bson.Serialization.Attributes;

namespace ConsumerAuditService.Models;

public class DenemeModel
{
    [BsonId]
    public string Id { get; set; }
    public string Deenem { get; set; }
    
}