using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hexure.Events.Namespace;
using MassTransit;

namespace Hexure.MassTransit.Events
{
    public class ConsumersEventTypeProviderBuilder : Hexure.Events.Serialization.EventTypeProviderBuilder
    {
        public ConsumersEventTypeProviderBuilder(IEventNamespaceReader eventNamespaceReader) : base(eventNamespaceReader)
        {
        }
        
        public ConsumersEventTypeProviderBuilder AddEventsFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            var result = assemblies
                .SelectMany(a => a.GetTypes())
                .SelectMany(t => t.GetInterfaces())
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))
                .SelectMany(t => t.GetGenericArguments())
                .GroupBy(type => type.Assembly)
                .ToList();

            foreach (var eventType in result)
            {
                AddEventsFromAssemblyOfType(eventType.First());
            }

            return this;
        }
    }
}