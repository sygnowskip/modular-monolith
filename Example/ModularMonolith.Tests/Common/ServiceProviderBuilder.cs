using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Tests.Common
{
    public class ServiceProviderBuilder
    {
        public static IServiceProvider Build()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();

            return serviceCollection.BuildServiceProvider();
        }
    }
}