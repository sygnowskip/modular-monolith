using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularMonolith.Orders.Domain;
using ModularMonolith.Orders.Domain.Policies;

namespace ModularMonolith.Orders.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOrdersWritePersistence<TDbContext>(this IServiceCollection services)
            where TDbContext : IOrdersDbContext
        {
            services.AddTransient<IOrdersDbContext>(provider => provider.GetService<TDbContext>());
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddOrdersDomain();
        }

        private static void AddOrdersDomain(this IServiceCollection services)
        {
            services.TryAddTransient<ISingleItemsCurrencyPolicy, SingleItemsCurrencyPolicy>();
        }
    }
}