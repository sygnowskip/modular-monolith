using Hexure.Events.Serialization;
using Hexure.MassTransit.Events.Serialization;
using MassTransit.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hexure.MassTransit.Events
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventsMassTransitSerializers(this IServiceCollection services)
        {
            services.TryAddTransient<IEventDeserializer, EventDeserializer>();
            services.TryAddTransient<IEventNameProvider, EventNamespaceNameProvider>();
            services.TryAddTransient<IMessageTypeProvider, EventNamespaceMessageTypeProvider>();
            services.TryAddTransient<IMessageTypeParser, EventNamespaceMessageTypeProvider>();
            services.TryAddTransient<EventNamespaceMessageSerializer>();
            services.TryAddTransient<EventNamespaceMessageDeserializer>(provider => new EventNamespaceMessageDeserializer(JsonMessageSerializer.Deserializer,
                provider.GetRequiredService<IEventTypeProvider>(),
                provider.GetRequiredService<IMessageTypeParser>()));

            return services;
        }
    }
}