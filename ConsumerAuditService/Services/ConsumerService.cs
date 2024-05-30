using Confluent.Kafka;
using ConsumerAuditService.Configs;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConsumerAuditService.Services;

public class ConsumerService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<ConsumerService> _logger;


    public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger)
    {
        _logger = logger;


        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = "OrderConsumerGroup",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        
        
    }

    public ConsumeResult<string,string> ProcessKafkaMessage(CancellationToken stoppingToken)
    {
        try
        {
            var consumeResult = _consumer.Consume(stoppingToken);
            return consumeResult;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing Kafka message: {ex.Message}");
            throw new Exception("Error in ProcessKafkaMessage in ConsumerService");
        }
    }

    public void Close()
    {
        _consumer.Close();
    }

    public void Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
    }
}