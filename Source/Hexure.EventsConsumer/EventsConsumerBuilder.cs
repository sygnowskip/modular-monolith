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
        private ConsumerRabbitMqSettings _rabbitMqSettings;

        private EventsConsumerBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public EventsConsumerBuilder WithHandlersFromAssemblyOfType<THandler>()
        {
            _consumerAssemblies.Add(typeof(THandler).Assembly);
            return this;
        }

        public EventsConsumerBuilder WithServices(Action<IServiceCollection> registerAction)
        {
            registerAction.Invoke(_serviceCollection);
            return this;
        }

        public EventsConsumerBuilder ToRabbitMq(ConsumerRabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
            return this;
        }

        public void Build()
        {
            _serviceCollection.RegisterRabbitMqConsumer(_rabbitMqSettings, _consumerAssemblies);
        }

        public static EventsConsumerBuilder Create(IServiceCollection serviceCollection)
        {
            return new EventsConsumerBuilder(serviceCollection);
        }
    }
}