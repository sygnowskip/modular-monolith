﻿using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<MonolithDbContext>(builder => builder.UseSqlServer(connectionString));
            return services;
        }
    }
}