using System;
using System.Collections.Generic;
using System.Reflection;
using Hexure.MassTransit.RabbitMq;
using Hexure.MassTransit.RabbitMq.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EventsConsumer
{
    public class EventsConsumerBuilder
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IList<Assembly> _consumerAssemblies = new List<Assembly>();
        private ConsumerRabbitMqSettings _consumerRabbitMqSettings;

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
            _consumerRabbitMqSettings = rabbitMqSettings;
            return this;
        }

        public EventsConsumer Build()
        {
            _serviceCollection.RegisterRabbitMqConsumer(_consumerRabbitMqSettings, _consumerAssemblies);
            return new EventsConsumer(_serviceCollection.BuildServiceProvider());
        }
    }
}