using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace ModularMonolith.Tests.Common
{
    public interface IHttpClientProvider
    {
        Task<HttpClient> PrepareClientWithTokenForScopesAsync(string name, string scopes = null);
        Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync();
        HttpContent Serialize<TRequest>(TRequest request);
        Task<TResult> DeserializeAsync<TResult>(HttpResponseMessage response);
        HttpClient CreateClient();
    }

    public class HttpClientProvider : IHttpClientProvider
    {
        protected HttpClient HttpClient => HttpClientFactory.CreateClient();
        
        protected readonly IHttpClientFactory HttpClientFactory;
        private readonly AuthoritySettings _authoritySettings;

        public HttpClientProvider(IHttpClientFactory httpClientFactory, AuthoritySettings authoritySettings)
        {
            HttpClientFactory = httpClientFactory;
            _authoritySettings = authoritySettings;
        }

        public HttpClient CreateClient() => HttpClientFactory.CreateClient();
        
        public async Task<HttpClient> PrepareClientWithTokenForScopesAsync(string name, string scopes = null)
        {
            var discoveryDocument = await GetDiscoveryDocumentAsync();
            var token = await HttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "modular-monolith-client",
                ClientSecret = "modular-monolith-client-secret",
                Scope = scopes
            });

            var authenticatedHttpClient = HttpClientFactory.CreateClient($"Authenticated_{name}");
            authenticatedHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return authenticatedHttpClient;
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
        
        public HttpContent Serialize<TRequest>(TRequest request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }

        public async Task<TResult> DeserializeAsync<TResult>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }
    }
}