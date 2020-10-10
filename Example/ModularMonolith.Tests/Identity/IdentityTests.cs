using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.Identity
{
    [TestFixture]
    public class IdentityTests : BaseHttpTests
    {
        [Test]
        public async Task ShouldBeAbleToGetTokenForClient()
        {
            var discoveryDocument = await GetDiscoveryDocumentAsync();
            var token = await HttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "modular-monolith-client",
                ClientSecret = "modular-monolith-client-secret",
                Scope = AuthoritySettings.Scopes
            });

            token.IsError.Should().BeFalse();
            token.AccessToken.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task ShouldNotGetTokenForNonExistingClient()
        {
            var discoveryDocument = await GetDiscoveryDocumentAsync();
            var token = await HttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "non-existing-client",
                ClientSecret = "non-existing-client-secret"
            });

            token.IsError.Should().BeTrue();
        }
    }
}