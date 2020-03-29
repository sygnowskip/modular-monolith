using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using ModularMonolith.Configuration;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.API
{
    [TestFixture]
    public class ScopesTests : BaseHttpTests
    {
        private readonly MonolithApiSettings _monolithApiSettings;
        private readonly AuthoritySettings _authoritySettings;

        public ScopesTests()
        {
            _authoritySettings = ApplicationSettingsConfigurationProvider.Get().GetSection("Authority").Get<AuthoritySettings>();
            _monolithApiSettings = ApplicationSettingsConfigurationProvider.Get().GetSection("Monolith").Get<MonolithApiSettings>();
        }

        [TestCase("api/registrations")]
        [TestCase("api/payments")]
        public async Task ShouldGet200WithToken(string relativeUrl)
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            
            var result = await httpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestCase("api/registrations")]
        [TestCase("api/payments")]
        public async Task ShouldNotAccessEndpointsWithoutToken(string relativeUrl)
        {
            var result = await HttpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [TestCase("api/registrations", "payments")]
        [TestCase("api/payments", "registrations")]
        public async Task ShouldNotAccessAddressWithDifferentScope(string relativeUrl, string scopes)
        {
            var httpClient = await PrepareClientWithTokenForScopes(scopes);

            var result = await httpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        private async Task<HttpClient> PrepareClientWithTokenForScopes(string scopes = null)
        {
            var discoveryDocument = await GetDiscoveryDocumentAsync(_authoritySettings.Url);
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