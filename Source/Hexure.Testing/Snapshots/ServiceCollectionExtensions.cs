using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.Testing.Snapshots
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSnapshots(this IServiceCollection serviceCollection,
            string connectionStringName)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new Snapshot(configuration.GetConnectionString(connectionStringName));
            });
            return serviceCollection;
        }
    }
}