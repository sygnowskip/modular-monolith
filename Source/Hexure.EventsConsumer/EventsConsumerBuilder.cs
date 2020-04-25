using System;
using System.Collections.Generic;
using System.Reflection;
using Hexure.RabbitMQ;
using Hexure.RabbitMQ.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EventsConsumer
{
    public class EventsConsumerBuilder
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IList<Assembly> _consumerAssemblies = new List<Assembly>();

        public EventsConsumerBuilder()
        {
            _serviceCollection = new ServiceCollection();
        }

        public EventsConsumerBuilder WithHandlersFromAssemblyOfType<THandler>()
        {
            _consumerAssemblies.Add(typeof(THandler).Assembly);
            return this;
        }

        public EventsConsumerBuilder WithDomain(Action<IServiceCollection> registerAction)
        {
            registerAction.Invoke(_serviceCollection);
            return this;
        }

        public EventsConsumerBuilder ToRabbitMq(ConsumerRabbitMqSettings rabbitMqSettings)
        {
            RabbitConnector.RegisterRabbitMqConsumer(_serviceCollection, rabbitMqSettings, _consumerAssemblies);
            return this;
        }

        public EventsConsumer Build()
        {
            return new EventsConsumer(_serviceCollection.BuildServiceProvider());
        }
    }
}