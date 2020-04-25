namespace Hexure.RabbitMQ.Settings
{
    public class ConsumerRabbitMqSettings : PublisherRabbitMqSettings
    {
        public string Queue { get; set; }
    }
}