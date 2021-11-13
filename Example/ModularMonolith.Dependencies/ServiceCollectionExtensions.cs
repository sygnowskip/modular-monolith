using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.CommandServices.Exams;
using ModularMonolith.Persistence;
using ModularMonolith.QueryServices;
using ModularMonolith.QueryServices.Common;

namespace ModularMonolith.Dependencies
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services.AddMediatR(typeof(CreateExamCommand).Assembly);
        }
        
        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(GetLocationsQuery).Assembly)
                .AddQueryServices();
        }

        public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection, string writeDatabaseConnectionString, string readDatabaseConnectionString)
        {
            return serviceCollection
                .AddWritePersistence(writeDatabaseConnectionString)
                .AddReadPersistence(readDatabaseConnectionString);
        }
    }
}
