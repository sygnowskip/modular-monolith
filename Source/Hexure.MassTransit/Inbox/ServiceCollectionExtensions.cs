using Hexure.EntityFrameworkCore.Inbox;
using Hexure.EntityFrameworkCore.Inbox.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.MassTransit.Inbox
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInbox<TInboxDbContext>(this IServiceCollection services)
            where TInboxDbContext : class, IInboxDbContext
        {
            services.AddTransient<IInboxDbContext>(provider => provider.GetRequiredService<TInboxDbContext>());
            services.AddTransient<IProcessedEventRepository, ProcessedEventRepository>();
            services.AddTransient<IConsumerProvider, ConsumerProvider>();
            services.AddTransient<IConsumerHashProvider, ConsumerHashProvider>();
            services.AddTransient<IConsumerPropertyProvider, ConsumerPropertyProvider>();
            return services;
        }
    }
}