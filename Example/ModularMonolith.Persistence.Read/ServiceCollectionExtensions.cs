using Hexure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Persistence.Read
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReadPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MonolithQueryDbContext>(builder =>
                builder
                    .UseSqlServer(connectionString)
                    .EnableIdentifiers()
            );
            return services;
        }
    }
}