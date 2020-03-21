using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Persistence.Read
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReadModelsPersistence(this IServiceCollection services)
        {
            //TODO: DI registration for domain components
            return services;
        }
    }
}