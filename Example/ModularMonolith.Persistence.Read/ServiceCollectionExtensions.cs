﻿using Hexure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;
using ModularMonolith.Persistence.Read.Validators;
using ModularMonolith.ReadModels;

namespace ModularMonolith.Persistence.Read
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReadPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MonolithQueryDbContext>(builder =>
                builder
                    .UseSqlServer(connectionString)
                    .EnableIdentifiers()
            );
            services.AddTransient<IMonolithQueryDbContext>(provider =>
                provider.GetRequiredService<MonolithQueryDbContext>());
            services.AddTransient<ILocationExistenceValidator, LocationExistenceValidator>();
            services.AddTransient<ISubjectExistenceValidator, SubjectExistenceValidator>();
            return services;
        }
    }
}