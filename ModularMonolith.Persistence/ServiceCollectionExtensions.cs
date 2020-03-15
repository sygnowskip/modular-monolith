using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            //TODO: DI registration for domain components
            return services;
        }
    }
}