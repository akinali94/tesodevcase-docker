namespace OrderService.Configs;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;    
    public string DatabaseName { get; set; } = null!;    
    public string OrdersCollectionName { get; set; } = null!;
}