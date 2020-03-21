using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using ModularMonolith.Tests.Common;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace ModularMonolith.Tests.Identity
{
    [TestFixture]
    public class IdentityTests : BaseHttpTests
    {
        private readonly AuthoritySettings _settings;
        private readonly HttpClient _httpClient;

        public IdentityTests()
        {
            _settings = ApplicationSettingsConfigurationProvider.Get().GetSection("Authority").Get<AuthoritySettings>();
            _httpClient = HttpClientFactory.CreateClient();
        }
        
        [Test]
        public async Task ShouldBeAbleToGetTokenForClient()
        {
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(_settings.Url);
            var token = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "modular-monolith-client",
                ClientSecret = "modular-monolith-client-secret"
            });

            token.IsError.Should().BeFalse();
            token.AccessToken.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task ShouldNotGetTokenForNonExistingClient()
        {
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(_settings.Url);
            var token = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "non-existing-client",
                ClientSecret = "non-existing-client-secret"
            });

            token.IsError.Should().BeTrue();
        }
    }
}