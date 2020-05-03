using System;
using Hexure.EntityFrameworkCore;
using Hexure.MediatR.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hexure.MediatR
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTransactionalCommands(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalCommandsBehavior<,>));
            services.TryAddTransient<ITransactionalBehaviorValidator, TransactionalBehaviorValidator>();
            return services;
        }

        public static IServiceCollection WithTransactionProvider(this IServiceCollection services, Func<IServiceProvider, ITransactionProvider> transactionProviderFactory)
        {
            services.TryAddTransient<ITransactionProvider>(transactionProviderFactory);
            return services;
        }
    }
}