using System;
using System.Diagnostics;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.SqlServer.Hints;
using Hexure.Events;
using Hexure.Events.Namespace;
using Hexure.Events.Serialization;
using Hexure.MassTransit.RabbitMq;
using Hexure.MassTransit.RabbitMq.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EventsPublisher
{
    public class EventsPublisherBuilder
    {
        private readonly EventTypeProviderBuilder _eventTypeProviderBuilder = new EventTypeProviderBuilder(new EventNamespaceReader());
        private readonly int _defaultBatchSize;
        private readonly TimeSpan _defaultDelay;

        private readonly IServiceCollection _serviceCollection;

        private PublisherRabbitMqSettings _publisherRabbitMqSettings;

        internal EventsPublisherBuilder(int defaultBatchSize, TimeSpan defaultDelay)
        {
            _defaultBatchSize = defaultBatchSize;
            _defaultDelay = defaultDelay;
            _serviceCollection = new ServiceCollection();
        }

        public EventsPublisherBuilder WithDbContext<TDbContext>()
            where TDbContext : ISerializedEventDbContext
        {
            _serviceCollection.AddTransient<ISerializedEventDbContext>(provider =>
                provider.GetRequiredService<TDbContext>());

            DiagnosticListener.AllListeners.Subscribe(new EntityFrameworkHintListener());

            return this;
        }

        public EventsPublisherBuilder ToRabbitMq(PublisherRabbitMqSettings rabbitMqSettings)
        {
            _publisherRabbitMqSettings = rabbitMqSettings;
            return this;
        }

        public EventsPublisherBuilder WithTransactionProvider<TTransactionProvider>()
            where TTransactionProvider : ITransactionProvider
        {
            _serviceCollection.AddTransient<ITransactionProvider>(provider =>
                provider.GetRequiredService<TTransactionProvider>());

            return this;
        }
        
        public EventsPublisherBuilder WithEventsFromAssemblyOfType<TType>()
            where TType : IEvent
        {
            _eventTypeProviderBuilder.AddEventsFromAssemblyOfType<TType>();
            return this;
        }

        public EventsPublisherBuilder WithPersistence(Action<IServiceCollection> services)
        {
            services(_serviceCollection);
            return this;
        }

        public EventsPublisher Build()
        {
            _serviceCollection.AddEventTypeProvider(_eventTypeProviderBuilder.Build());
            _serviceCollection.RegisterRabbitMqPublisher(_publisherRabbitMqSettings);
            return new EventsPublisher(_defaultBatchSize, _defaultDelay, _serviceCollection.BuildServiceProvider());
        }
    }
}