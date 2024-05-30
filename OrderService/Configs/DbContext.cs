using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Models;

namespace OrderService.Configs;

public class DbContext
{
    private readonly IMongoDatabase _database;
    //public IMongoCollection<Order> _collection;

    public DbContext(IOptions<MongoDbSettings> orderDbSettings)
    {
        var mongoClient = new MongoClient(orderDbSettings.Value.ConnectionString);
        _database = mongoClient.GetDatabase(orderDbSettings.Value.DatabaseName);
        //_collection = mongoDatabase.GetCollection<Order>(orderDbSettings.Value.OrdersCollectionName);
        
    }
    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");

}