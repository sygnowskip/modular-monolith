using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Configuration;

namespace ModularMonolith.Tests.Common
{
    public abstract class BaseHttpTests
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IHttpClientFactory HttpClientFactory;
        protected HttpClient HttpClient => HttpClientFactory.CreateClient();

        private readonly AuthoritySettings _authoritySettings;
        protected readonly MonolithApiSettings MonolithSettings;

        protected BaseHttpTests()
        {
            ServiceProvider = ServiceProviderBuilder.Build();
            HttpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
            _authoritySettings = ApplicationSettingsConfigurationProvider.Get().GetSection("Authority").Get<AuthoritySettings>();
            MonolithSettings = ApplicationSettingsConfigurationProvider.Get().GetSection("Monolith").Get<MonolithApiSettings>();
        }

        public async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync()
        {
            return await HttpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _authoritySettings.Url,
                Policy = new DiscoveryPolicy()
                {
                    ValidateIssuerName = false
                }
            });
        }

        public async Task<HttpClient> PrepareClientWithTokenForScopes(string scopes = null)
        {
            var discoveryDocument = await GetDiscoveryDocumentAsync();
            var token = await HttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "modular-monolith-client",
                ClientSecret = "modular-monolith-client-secret",
                Scope = scopes
            });

            var authenticatedHttpClient = HttpClientFactory.CreateClient("Authenticated");
            authenticatedHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return authenticatedHttpClient;
        }
    }
}