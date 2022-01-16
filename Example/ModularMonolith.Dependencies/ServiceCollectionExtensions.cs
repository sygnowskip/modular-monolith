using Hexure.Time;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularMonolith.CommandServices.Exams;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Persistence;
using ModularMonolith.QueryServices;
using ModularMonolith.QueryServices.Common;

namespace ModularMonolith.Dependencies
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.TryAddTransient<ISystemTimeProvider, SystemTimeProvider>();
            services.TryAddTransient<ISingleCurrencyPolicy, SingleCurrencyPolicy>();
            return services;
        }
        
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
