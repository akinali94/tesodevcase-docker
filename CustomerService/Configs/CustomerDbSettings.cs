namespace CustomerService.Configs;

public class CustomerDbSettings
{
    public string ConnectionString { get; set; } = null!;    
    public string DatabaseName { get; set; } = null!;    
    public string CustomersCollectionName { get; set; } = null!;
}