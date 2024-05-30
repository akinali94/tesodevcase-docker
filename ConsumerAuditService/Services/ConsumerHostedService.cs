using ConsumerAuditService.Configs;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConsumerAuditService.Services;

public class ConsumerHostedService : BackgroundService
{
    private readonly ConsumerService _consumer;
    private readonly IMongoCollection<BsonDocument> _collection;

    public ConsumerHostedService(ConsumerService consumer, IOptions<MongoDbSettings> dbSettings)
    {
        _consumer = consumer;
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _collection = database.GetCollection<BsonDocument>(dbSettings.Value.AuditCollectionName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("order-logs");
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = _consumer.ProcessKafkaMessage(stoppingToken);
            
            if (consumeResult != null)
            {
                var message = consumeResult.Message.Value;
                var bsonMessage = BsonDocument.Parse(message);
                await _collection.InsertOneAsync(bsonMessage,null,stoppingToken);
            }
            
            //await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _consumer.Close();
    }
}

/*
 *
 *     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
   {
       while (!stoppingToken.IsCancellationRequested)
       {
           ProcessKafkaMessage(stoppingToken);
           await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
       }

       _consumer.Close();
   }
*/