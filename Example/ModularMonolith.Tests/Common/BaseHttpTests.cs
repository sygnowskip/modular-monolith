using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Tests.Common
{
    public abstract class BaseHttpTests
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IHttpClientFactory HttpClientFactory;
        protected HttpClient HttpClient => HttpClientFactory.CreateClient();

        protected BaseHttpTests()
        {
            ServiceProvider = ServiceProviderBuilder.Build();
            HttpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }

        public async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(string authorityUrl)
        {
            return await HttpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = authorityUrl,
                Policy = new DiscoveryPolicy()
                {
                    ValidateIssuerName = false
                }
            });
        }
    }
}