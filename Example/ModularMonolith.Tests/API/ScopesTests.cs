using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.API
{
    [TestFixture]
    public class ScopesTests : BaseHttpTests
    {
        [TestCase("api/registrations")]
        [TestCase("api/payments")]
        public async Task ShouldGet200WithToken(string relativeUrl)
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            
            var result = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestCase("api/registrations")]
        [TestCase("api/payments")]
        public async Task ShouldNotAccessEndpointsWithoutToken(string relativeUrl)
        {
            var result = await HttpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [TestCase("api/registrations", "payments")]
        [TestCase("api/payments", "registrations")]
        public async Task ShouldNotAccessAddressWithDifferentScope(string relativeUrl, string scopes)
        {
            var httpClient = await PrepareClientWithTokenForScopes(scopes);

            var result = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, relativeUrl));
            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}