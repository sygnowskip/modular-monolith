namespace Hexure.MassTransit.RabbitMq.Settings
{
    public class ConsumerRabbitMqSettings : PublisherRabbitMqSettings
    {
        public string Queue { get; set; }
    }
}