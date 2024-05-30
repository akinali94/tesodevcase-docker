using Confluent.Kafka;
using MongoDB.Bson.IO;

namespace OrderService.Configs;

public class KafkaProducerConfig : IKafkaProducerConfig //ProducerService
{
    private readonly IConfiguration _configuration;
    private readonly IProducer<string, string> _producer;

    public KafkaProducerConfig(IConfiguration configuration)
    {
        _configuration = configuration;

        var producerConfig = new ProducerConfig()
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task ProduceAsync(string topic, object message)
    {
        var kafkaMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = Newtonsoft.Json.JsonConvert.SerializeObject(message)
        };
        await _producer.ProduceAsync(topic, kafkaMessage);
    }
}