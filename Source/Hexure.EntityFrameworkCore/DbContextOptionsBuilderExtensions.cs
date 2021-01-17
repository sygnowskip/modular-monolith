using System;
using Hexure.EntityFrameworkCore.Deleting;
using Hexure.EntityFrameworkCore.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder AddPublishDomainEventsInterceptorOnSaveChanges(this DbContextOptionsBuilder builder, IServiceProvider provider)
        {
            return builder
                    .AddInterceptors(provider.GetRequiredService<PublishDomainEventsOnSaveChangesInterceptor>());
        }
        
        public static DbContextOptionsBuilder AddDeleteAggregatesInterceptorOnSaveChanges(this DbContextOptionsBuilder builder, IServiceProvider provider)
        {
            return builder
                    .AddInterceptors(provider.GetRequiredService<DeleteAggregatesOnSaveChangesInterceptor>());
        }
    }
}