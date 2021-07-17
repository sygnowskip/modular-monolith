using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.CommandServices.Exams;
using ModularMonolith.Payments.ApplicationServices;
using ModularMonolith.Persistence;
using ModularMonolith.QueryServices;
using ModularMonolith.QueryServices.Common;
using ModularMonolith.Registrations.ApplicationServices;
using ModularMonolith.Registrations.Queries;

namespace ModularMonolith.Dependencies
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandServices(this IServiceCollection services)
        {
            return services.AddMediatR(typeof(CreateExamCommand).Assembly);
        }
        
        public static IServiceCollection AddQueryServices(this IServiceCollection services)
        {
            return services.AddMediatR(typeof(GetLocationsQuery).Assembly);
        }
        
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

        public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection, string writeDatabaseConnectionString, string readDatabaseConnectionString)
        {
            return serviceCollection
                .AddWritePersistence(writeDatabaseConnectionString)
                .AddReadPersistence(readDatabaseConnectionString);
        }
    }
}
