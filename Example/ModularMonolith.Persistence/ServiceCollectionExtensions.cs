using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.SqlServer.Events;
using Hexure.Events;
using Hexure.MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Payments;
using ModularMonolith.Persistence.Repositories;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWritePersistence(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddDbContext<MonolithDbContext>(builder => 
                builder
                    .UseSqlServer(connectionString)
                    .EnableIdentifiers()
                );

            services.AddTransactionalCommands()
                .WithTransactionProvider(provider => provider.GetRequiredService<MonolithDbContext>());

            services.AddDomainEvents()
                .WithPersistence<MonolithDbContext>();

            return services;
        }
    }
}