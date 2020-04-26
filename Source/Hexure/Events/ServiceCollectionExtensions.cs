using Hexure.Events.Namespace;
using Hexure.Events.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hexure.Events
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.TryAddTransient<IEventSerializer, EventSerializer>();
            services.TryAddTransient<IEventNamespaceReader, EventNamespaceReader>();
            return services;
        }

        public static IServiceCollection AddEventTypeProvider(this IServiceCollection services, IEventTypeProvider eventTypeProvider)
        {
            services.TryAddTransient<IEventTypeProvider>(_ => eventTypeProvider);
            return services;
        }
    }
}