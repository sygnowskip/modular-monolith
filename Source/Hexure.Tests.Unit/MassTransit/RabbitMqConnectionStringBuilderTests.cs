using FluentAssertions;
using Hexure.MassTransit;
using NUnit.Framework;

namespace Hexure.Tests.Unit.MassTransit
{
    [TestFixture]
    public class RabbitMqConnectionStringBuilderTests
    {
        [Test]
        public void ShouldReturnCorrectRabbitMqConnectionString()
        {
            var protocol = "rabbitmq://";
            var host = "localhost:15672";
            var username = "admin";
            var password = "qwerty123";

            var connectionString = RabbitMqConnectionStringBuilder.Build($"{protocol}{host}", username, password);

            connectionString.Should()
                .Be(
                    $"{RabbitMqConnectionStringBuilder.DefaultProtocol}://{username}:{password}@{host}/{RabbitMqConnectionStringBuilder.DefaultVirtualHost}");
        }
        
        [Test]
        public void ShouldReturnCorrectRabbitMqConnectionStringForHostWithoutProtocol()
        {
            var host = "localhost:15672";
            var username = "admin";
            var password = "qwerty123";

            var connectionString = RabbitMqConnectionStringBuilder.Build(host, username, password);

            connectionString.Should()
                .Be(
                    $"{RabbitMqConnectionStringBuilder.DefaultProtocol}://{username}:{password}@{host}/{RabbitMqConnectionStringBuilder.DefaultVirtualHost}");
        }
    }
}