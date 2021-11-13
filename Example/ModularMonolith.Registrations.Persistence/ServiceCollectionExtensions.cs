using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Registrations.Domain;

namespace ModularMonolith.Registrations.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void AddExamsWritePersistence<TDbContext>(this IServiceCollection services)
            where TDbContext : IRegistrationsDbContext
        {
            services.AddTransient<IRegistrationsDbContext>(provider => provider.GetService<TDbContext>());
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
        }
    }
}