using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hexure.Testing.Resources
{
    public class RabbitMqResourceConfiguration
    {
        public string RabbitMqManagementUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RabbitMqResourceAwaiter
    {
        private readonly HttpClient _httpClient;
        private readonly RabbitMqResourceConfiguration _configuration;
        private readonly ILogger<RabbitMqResourceAwaiter> _logger;

        public RabbitMqResourceAwaiter(HttpClient httpClient, IOptions<RabbitMqResourceConfiguration> configuration, ILogger<RabbitMqResourceAwaiter> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration.Value;
            ConfigureHttpClientAuthentication();
        }

        public async Task WaitForConnectionAsync()
        {
            var alivenessUri = new Uri(new Uri(_configuration.RabbitMqManagementUrl), "api/aliveness-test/%2F");
            var acceptingConnections = false;
            while (!acceptingConnections)
            {
                try
                {
                    var result = await _httpClient.GetAsync(alivenessUri);
                    if (result.IsSuccessStatusCode)
                    {
                        acceptingConnections = true;
                        _logger.LogInformation($"Connection accepted! ({_configuration.RabbitMqManagementUrl})");
                    }
                }
                catch
                {
                    acceptingConnections = false;
                    _logger.LogInformation($"Connection refused, retrying... ({_configuration.RabbitMqManagementUrl})");
                }

                await Task.Delay(TimeSpan.FromMilliseconds(2500));
            }
        }

        private void ConfigureHttpClientAuthentication()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes($"{_configuration.Username}:{_configuration.Password}")));
        }
    }
}