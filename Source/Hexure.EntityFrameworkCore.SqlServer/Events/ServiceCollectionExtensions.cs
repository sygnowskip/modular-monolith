using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EntityFrameworkCore.SqlServer.Events
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection WithPersistence<TDbContext>(this IServiceCollection services)
            where TDbContext : class, ISerializedEventDbContext
        {
            services.AddTransient<ISerializedEventDbContext>(provider => provider.GetRequiredService<TDbContext>());
            services.AddTransient<ISerializedEventRepository, SerializedEventRepository>();
            return services;
        }
    }
}