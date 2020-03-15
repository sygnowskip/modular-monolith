using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Registrations.ApplicationServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRegistrations(this IServiceCollection services)
        {
            //TODO: DI registration for domain components
            return services;
        }
    }
}