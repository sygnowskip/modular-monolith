using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Hexure.Testing.Resources
{
    public class WebApiResourceAwaiter
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IdentityServerAwaiter> _logger;

        public WebApiResourceAwaiter(HttpClient httpClient, ILogger<IdentityServerAwaiter> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task WaitForEndpointAsync(string url)
        {
            var acceptingConnections = false;
            while (!acceptingConnections)
            {
                try
                {
                    var result = await _httpClient.GetAsync(url);
                    if (result.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Connection accepted! ({url})");
                        acceptingConnections = true;
                    }
                }
                catch
                {
                    acceptingConnections = false;
                    _logger.LogInformation($"Connection refused, retrying... ({url})");
                }

                await Task.Delay(TimeSpan.FromMilliseconds(2500));
            }
        }
    }
}