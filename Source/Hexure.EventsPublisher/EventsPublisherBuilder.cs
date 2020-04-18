using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Collecting;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Hexure.EntityFrameworkCore.SqlServer.Events;
using Hexure.EntityFrameworkCore.SqlServer.Hints;
using Hexure.Events;
using Hexure.Events.Namespace;
using Hexure.Events.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EventsPublisher
{
    public class EventsPublisherBuilder
    {
        private readonly IEventNamespaceReader _eventNamespaceReader = new EventNamespaceReader(); 
        private readonly Dictionary<string, Assembly> _namespaces = new Dictionary<string, Assembly>();
        private readonly int _defaultBatchSize;
        private readonly TimeSpan _defaultDelay;

        private readonly IServiceCollection _serviceCollection;

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

        public EventsPublisherBuilder WithTransactionProvider<TTransactionProvider>()
            where TTransactionProvider : ITransactionProvider
        {
            _serviceCollection.AddTransient<ITransactionProvider>(provider =>
                provider.GetRequiredService<TTransactionProvider>());

            return this;
        }

        public EventsPublisherBuilder WithDomain(Action<IServiceCollection> registerDomainAction)
        {
            registerDomainAction(_serviceCollection);
            return this;
        }
        public EventsPublisherBuilder WithEventsFromAssemblyOfType<TType>()
            where TType : IEvent
        {
            var ns = _eventNamespaceReader.GetFromAssemblyOfType<TType>();
            if (ns.HasNoValue)
                throw new InvalidOperationException("Unable to publish events from assembly without [EventNamespace] attribute");

            if (_namespaces.ContainsKey(ns.Value.Name))
                throw new InvalidOperationException("Unable to publish events from assemblies with duplicated values of [EventNamespace] attribute");

            _namespaces.Add(ns.Value.Name, typeof(TType).Assembly);

            return this;
        }

        public EventsPublisher Build()
        {
            RegisterCommonServices();

            return new EventsPublisher(_defaultBatchSize, _defaultDelay, _serviceCollection.BuildServiceProvider());
        }

        private void RegisterCommonServices()
        {
            _serviceCollection.AddTransient<IEventDeserializer, EventDeserializer>();
            _serviceCollection.AddSingleton<IEventTypeProvider>(new EventTypeProvider(_namespaces));
        }
    }
}