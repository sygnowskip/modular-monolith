using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Collecting;
using Hexure.EntityFrameworkCore.Events.Publishing;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Hexure.Events.Publishing;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EntityFrameworkCore.SqlServer.Events
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection WithPersistence<TDbContext>(this IServiceCollection services)
            where TDbContext : class, ISerializedEventDbContext
        {
            services.AddTransient<ISerializedEventDbContext>(provider => provider.GetRequiredService<TDbContext>());
            services.AddTransient<IEventPublisher, DatabaseEventPublisher>();
            services.AddTransient<ISerializedEventRepository, SerializedEventRepository>();
            services.AddTransient<IEventCollector, EventCollector>();
            return services;
        }
    }
}