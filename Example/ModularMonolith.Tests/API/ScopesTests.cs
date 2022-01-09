using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.API
{
    [TestFixture]
    public class ScopesTests : BaseScenariosTests
    {
        [TestCase("api/registrations")]
        [TestCase("api/payments")]
        public async Task ShouldGet200WithToken(string relativeUrl)
        {
            var httpClient = await HttpClientProvider.PrepareClientWithTokenForScopesAsync("Scopes");
            
            var result = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestCase("api/registrations")]
        [TestCase("api/payments")]
        public async Task ShouldNotAccessEndpointsWithoutToken(string relativeUrl)
        {
            var httpClient = await HttpClientProvider.PrepareClientWithTokenForScopesAsync("Scopes");
            var result = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [TestCase("api/registrations", "payments")]
        [TestCase("api/payments", "registrations")]
        public async Task ShouldNotAccessAddressWithDifferentScope(string relativeUrl, string scopes)
        {
            var httpClient = await HttpClientProvider.PrepareClientWithTokenForScopesAsync("Scopes");

            var result = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}