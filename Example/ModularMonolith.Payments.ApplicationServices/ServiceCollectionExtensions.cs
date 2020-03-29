using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Payments.Contracts;

namespace ModularMonolith.Payments.ApplicationServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentsServices(this IServiceCollection services)
        {
            services.AddTransient<IPaymentsApplicationService, PaymentsApplicationService>();
            return services;
        }
    }
}