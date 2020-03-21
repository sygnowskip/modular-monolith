using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Tests.Common
{
    public abstract class BaseHttpTests
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IHttpClientFactory HttpClientFactory;

        protected BaseHttpTests()
        {
            ServiceProvider = ServiceProviderBuilder.Build();
            HttpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }
    }
}