using System;
using Hexure.EntityFrameworkCore.Identifiers;
using Hexure.EntityFrameworkCore.SqlServer.Events;
using Hexure.Events;
using Hexure.MediatR;
using Hexure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularMonolith.Exams.Persistence;
using ModularMonolith.Payments;
using ModularMonolith.Persistence.Repositories;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    public class SystemTimeProviderBindingFactory : IParameterBindingFactory
    {
        public bool CanBind(Type parameterType, string parameterName)
        {
            return parameterType == typeof(ISystemTimeProvider);
        }

        public ParameterBinding Bind(IMutableEntityType entityType, Type parameterType, string parameterName)
        {
            throw new NotImplementedException();
        }

        public ParameterBinding Bind(IConventionEntityType entityType, Type parameterType, string parameterName)
        {
            throw new NotImplementedException();
        }
    }
    
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWritePersistence(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddDbContext<MonolithDbContext>(builder =>
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkSqlServer()
                        .AddTransient<ISystemTimeProvider, SystemTimeProvider>()
                        .AddTransient<IParameterBindingFactory>(provider => new ServiceParameterBindingFactory(typeof(ISystemTimeProvider)))
                        .Replace(ServiceDescriptor.Transient<IValueConverterSelector, IdentifiersValueConverterSelector>())
                        .BuildServiceProvider();

                    builder
                        .UseSqlServer(connectionString)
                        .UseInternalServiceProvider(serviceProvider);
                }
            );

            services.AddExamsWritePersistence<MonolithDbContext>();

            services.AddTransactionalCommands()
                .WithTransactionProvider(provider => provider.GetRequiredService<MonolithDbContext>());

            services.AddDomainEvents()
                .WithPersistence<MonolithDbContext>();

            return services;
        }
    }
}