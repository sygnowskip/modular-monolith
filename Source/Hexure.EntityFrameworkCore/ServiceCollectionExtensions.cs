using Hexure.EntityFrameworkCore.Deleting;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hexure.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection EnableIdentifiers(this IServiceCollection services)
        {
            return services
                .Replace(ServiceDescriptor.Transient<IValueConverterSelector, IdentifiersValueConverterSelector>());
        }
        
        public static IServiceCollection AddInterceptors(this IServiceCollection services)
        {
            services.TryAddTransient<DeleteAggregatesOnSaveChangesInterceptor>();
            services.TryAddTransient<PublishDomainEventsOnSaveChangesInterceptor>();

            return services;
        }
    }
}