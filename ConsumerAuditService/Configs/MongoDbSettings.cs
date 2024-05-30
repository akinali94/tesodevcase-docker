namespace ConsumerAuditService.Configs;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;    
    public string DatabaseName { get; set; } = null!;    
    public string AuditCollectionName { get; set; } = null!;
}