using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Configuration;
using Newtonsoft.Json;

namespace ModularMonolith.Tests.Common
{
    public abstract class BaseHttpTests
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IHttpClientFactory HttpClientFactory;
        protected HttpClient HttpClient => HttpClientFactory.CreateClient();

        protected readonly AuthoritySettings AuthoritySettings;
        protected readonly MonolithApiSettings MonolithSettings;

        protected BaseHttpTests()
        {
            ServiceProvider = ServiceProviderBuilder.Build();
            HttpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
            AuthoritySettings = ApplicationSettingsConfigurationProvider.Get().GetSection("Authority").Get<AuthoritySettings>();
            MonolithSettings = ApplicationSettingsConfigurationProvider.Get().GetSection("Monolith").Get<MonolithApiSettings>();
        }

        protected async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync()
        {
            return await HttpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = AuthoritySettings.Url,
                Policy = new DiscoveryPolicy()
                {
                    ValidateIssuerName = false
                }
            });
        }

        protected async Task<HttpClient> PrepareClientWithTokenForScopes(string scopes = null)
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

        protected HttpContent Serialize<TRequest>(TRequest request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }
        
        protected async Task<TResult> DeserializeAsync<TResult>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());

        }
    }
}