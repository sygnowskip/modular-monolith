using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Payments.ApplicationServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPayments(this IServiceCollection services)
        {
            //TODO: DI registration for domain components
            return services;
        }
    }
}