using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConsumerAuditService.Configs;

public class DbContext
{
    private readonly IMongoDatabase _database;

    public DbContext(IOptions<MongoDbSettings> auditDbSettings)
    {
        var mongoClient = new MongoClient(auditDbSettings.Value.ConnectionString);
        _database = mongoClient.GetDatabase(auditDbSettings.Value.DatabaseName);
        
    }

    public IMongoCollection<BsonDocument> AuditLogs => _database.GetCollection<BsonDocument>("AuditLogs");
}