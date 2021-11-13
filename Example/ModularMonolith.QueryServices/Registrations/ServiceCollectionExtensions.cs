using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.QueryServices.Registrations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRegistrationQueries(this IServiceCollection services)
        {
            //services.AddMediatR(typeof(GetSingleRegistrationQueryHandler).Assembly);
            return services;
        }
    }
}