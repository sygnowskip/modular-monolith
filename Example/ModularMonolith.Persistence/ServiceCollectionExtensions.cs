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
using ModularMonolith.Payments;
using ModularMonolith.Persistence.Repositories;
using ModularMonolith.Persistence.Validators;
using ModularMonolith.ReadModels;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWritePersistence(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddDbContext<MonolithDbContext>((provider, builder) =>
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkSqlServer()
                        .AddTransient<ISystemTimeProvider, SystemTimeProvider>()
                        .AddTransient<IParameterBindingFactory>(sp => new ServiceParameterBindingFactory(typeof(ISystemTimeProvider)))
                        .EnableIdentifiers()
                        .BuildServiceProvider();

                    builder
                        .UseSqlServer(connectionString)
                        .UseInternalServiceProvider(serviceProvider)
                        .AddPublishDomainEventsInterceptorOnSaveChanges(provider)
                        .AddDeleteAggregatesInterceptorOnSaveChanges(provider);
                }
            );

            services.AddInterceptors();
            services.AddExamsWritePersistence<MonolithDbContext>();

            services.AddTransactionalCommands()
                .WithTransactionProvider(provider => provider.GetRequiredService<MonolithDbContext>());

            services.AddDomainEvents()
                .WithPersistence<MonolithDbContext>();
            
            services.AddTransient<IMonolithQueryDbContext>(provider =>
                provider.GetRequiredService<MonolithDbContext>());
            services.AddTransient<ILocationExistenceValidator, LocationExistenceValidator>();
            services.AddTransient<ISubjectExistenceValidator, SubjectExistenceValidator>();
            services.AddTransient<IExamExistenceValidator, ExamExistenceValidator>();

            return services;
        }
    }
}