namespace OrderService.Configs;

public interface IKafkaProducerConfig
{
    Task ProduceAsync(string topic, object message);
}