using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.QueryServices.Exams;

namespace ModularMonolith.QueryServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueryServices(this IServiceCollection services)
        {
            services.AddTransient<IQueryBuilder, QueryBuilder>();
            return services;
        }
    }
}