using Hexure.Events.Namespace;
using Hexure.Events.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.Events
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddTransient<IEventSerializer, EventSerializer>();
            services.AddTransient<IEventNamespaceReader, EventNamespaceReader>();
            return services;
        }
    }
}