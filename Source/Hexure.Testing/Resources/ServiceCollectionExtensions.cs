using Microsoft.Extensions.DependencyInjection;

namespace Hexure.Testing.Resources
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddResourceAwaiters(this IServiceCollection serviceCollection, RabbitMqResourceConfiguration rabbitMqResourceConfiguration)
        {
            serviceCollection.AddTransient<IdentityServerAwaiter>();
            serviceCollection.AddHttpClient<IdentityServerAwaiter>();
            
            serviceCollection.AddTransient<RabbitMqResourceAwaiter>();
            serviceCollection.AddHttpClient<RabbitMqResourceAwaiter>();
            serviceCollection.AddTransient<RabbitMqResourceConfiguration>(provider => rabbitMqResourceConfiguration);
            
            serviceCollection.AddTransient<WebApiResourceAwaiter>();
            serviceCollection.AddHttpClient<WebApiResourceAwaiter>();
            
            serviceCollection.AddTransient<MsSqlResourceAwaiter>();
            return serviceCollection;
        }
    }
}