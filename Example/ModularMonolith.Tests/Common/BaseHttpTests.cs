using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Tests.Common
{
    public abstract class BaseHttpTests
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly HttpClient HttpClient;

        protected BaseHttpTests()
        {
            ServiceProvider = ServiceProviderBuilder.Build();
            HttpClient = ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
        }
    }
}