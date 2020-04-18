using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Collecting;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EntityFrameworkCore.SqlServer.Events
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection WithPersistence<TDbContext>(this IServiceCollection services)
            where TDbContext : class, ISerializedEventDbContext
        {
            services.AddTransient<ISerializedEventDbContext, TDbContext>();
            services.AddTransient<ISerializedEventRepository, SerializedEventRepository>();
            services.AddTransient<IEventCollector, EventCollector>();
            return services;
        }
    }
}