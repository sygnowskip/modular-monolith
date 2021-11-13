using Hexure.Dapper;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.SqlServer.Events;
using Hexure.Events;
using Hexure.MediatR;
using Hexure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Exams.Language.Validators;
using ModularMonolith.Exams.Persistence;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;
using ModularMonolith.Persistence.Validators;
using ModularMonolith.ReadModels;
using ModularMonolith.Registrations.Domain;

namespace ModularMonolith.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReadPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbConnection(connectionString);
            return services;
        }
        
        public static IServiceCollection AddWritePersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MonolithDbContext>((provider, builder) =>
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkSqlServer()
                        .AddTransient<ISystemTimeProvider, SystemTimeProvider>()
                        .AddTransient<IParameterBindingFactory>(sp =>
                            new ServiceParameterBindingFactory(typeof(ISystemTimeProvider)))
                        .EnableIdentifiers()
                        .BuildServiceProvider();

                    builder
                        .UseSqlServer(connectionString)
                        .UseInternalServiceProvider(serviceProvider)
                        .AddPublishDomainEventsInterceptorOnSaveChanges(provider)
                        .AddDeleteAggregatesInterceptorOnSaveChanges(provider);
                }
            ).WithTransactionProvider(provider => provider.GetRequiredService<MonolithDbContext>());

            services.AddInterceptors();
            services.AddExamsWritePersistence<MonolithDbContext>();

            services.AddDomainEvents()
                .WithPersistence<MonolithDbContext>();

            services.AddTransient<IMonolithQueryDbContext>(provider =>
                provider.GetRequiredService<MonolithDbContext>());
            services.AddTransient<ILocationExistenceValidator, LocationExistenceValidator>();
            services.AddTransient<ISubjectExistenceValidator, SubjectExistenceValidator>();

            return services;
        }
    }
}