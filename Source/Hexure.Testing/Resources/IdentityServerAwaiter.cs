using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hexure.Testing.Resources
{
    public class IdentityServerAwaiterDiscoveryDocument
    {
        [JsonProperty("scopes_supported")] public ICollection<string> ScopesSupported { get; set; }
    }

    public class IdentityServerAwaiter
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IdentityServerAwaiter> _logger;

        public IdentityServerAwaiter(ILogger<IdentityServerAwaiter> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task WaitForScopeAsync(string authorityUri, string scope)
        {
            var discoveryDocumentUri = new Uri(new Uri(authorityUri), "/.well-known/openid-configuration");
            var acceptingConnectionsAndScopeExist = false;
            while (!acceptingConnectionsAndScopeExist)
            {
                try
                {
                    var result = await _httpClient.GetAsync(discoveryDocumentUri);
                    if (result.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Connection accepted! ({discoveryDocumentUri.AbsoluteUri})");
                        if (await ScopeExist(result, scope))
                        {
                            _logger.LogInformation($"Scope exist!");
                            acceptingConnectionsAndScopeExist = true;
                        }
                        else
                        {
                            _logger.LogInformation($"Scope does not exist, retrying...");
                        }
                    }
                }
                catch
                {
                    acceptingConnectionsAndScopeExist = false;
                    _logger.LogInformation($"Connection refused, retrying... ({discoveryDocumentUri.AbsoluteUri})");
                }

                await Task.Delay(TimeSpan.FromMilliseconds(2500));
            }
        }

        private async Task<bool> ScopeExist(HttpResponseMessage result, string scope)
        {
            var response = await result.Content.ReadAsStringAsync();
            var discoveryDocument = JsonConvert.DeserializeObject<IdentityServerAwaiterDiscoveryDocument>(response);
            return discoveryDocument.ScopesSupported != null && discoveryDocument.ScopesSupported.Contains(scope);
        }
    }
}