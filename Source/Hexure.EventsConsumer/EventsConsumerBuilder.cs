using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hexure.Events.Namespace;
using Hexure.Events.Serialization;
using Hexure.RabbitMQ;
using Hexure.RabbitMQ.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EventsConsumer
{
    public class EventsConsumerBuilder
    {
        private readonly EventTypeProviderBuilder _eventTypeProviderBuilder = new EventTypeProviderBuilder(new EventNamespaceReader());
        private readonly IServiceCollection _serviceCollection;
        private readonly IList<Assembly> _consumerAssemblies = new List<Assembly>();

        public EventsConsumerBuilder()
        {
            _serviceCollection = new ServiceCollection();
        }

        public EventsConsumerBuilder WithHandlersFromAssemblyOfType<THandler>()
        {
            _consumerAssemblies.Add(typeof(THandler).Assembly);
            AddEventsFromAssemblyOfConsumer<THandler>();
            return this;
        }

        private void AddEventsFromAssemblyOfConsumer<TConsumer>()
        {
            var result = typeof(TConsumer).Assembly
                .GetTypes()
                .SelectMany(t => t.GetInterfaces())
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))
                .SelectMany(t => t.GetGenericArguments())
                .GroupBy(type => type.Assembly)
                .ToList();

            foreach (var eventType in result)
            {
                _eventTypeProviderBuilder.AddEventsFromAssemblyOfType(eventType.First());
            }
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
            RegisterCommonServices();

            return new EventsConsumer(_serviceCollection.BuildServiceProvider());
        }

        private void RegisterCommonServices()
        {
            _serviceCollection.AddTransient<IEventDeserializer, EventDeserializer>();
            _serviceCollection.AddSingleton<IEventTypeProvider>(_eventTypeProviderBuilder.Build());
        }
    }
}