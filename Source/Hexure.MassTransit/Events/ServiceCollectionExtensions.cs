using Hexure.Events;
using Hexure.Events.Serialization;
using Hexure.MassTransit.Events.Serialization;
using MassTransit.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hexure.MassTransit.Events
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEventsMassTransitSerializers(this IServiceCollection services)
        {
            services.AddDomainEvents();
            services.TryAddTransient<IEventDeserializer, EventDeserializer>();
            services.TryAddTransient<IEventNameProvider, EventNamespaceNameProvider>();
            services.TryAddTransient<IMessageTypeProvider, EventNamespaceMessageTypeProvider>();
            services.TryAddTransient<IMessageTypeParser, EventNamespaceMessageTypeProvider>();
            services.TryAddTransient<EventNamespaceMessageSerializer>();
            services.TryAddTransient<EventNamespaceMessageDeserializer>(provider => new EventNamespaceMessageDeserializer(JsonMessageSerializer.Deserializer,
                provider.GetRequiredService<IEventTypeProvider>(),
                provider.GetRequiredService<IMessageTypeParser>()));
        }
    }
}