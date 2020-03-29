using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Payments.ApplicationServices;
using ModularMonolith.Persistence;
using ModularMonolith.Persistence.Read;
using ModularMonolith.Registrations.ApplicationServices;
using ModularMonolith.Registrations.Queries;

namespace ModularMonolith.Dependencies
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRegistrations(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddRegistrationServices()
                .AddRegistrationQueries();
        }

        public static IServiceCollection AddPayments(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddPaymentsServices();
        }

        public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection,
            string writeDatabaseConnectionString, string readDatabaseConnectionString)
        {
            return serviceCollection
                .AddWritePersistence(writeDatabaseConnectionString)
                .AddReadPersistence(readDatabaseConnectionString);
        }
    }
}
