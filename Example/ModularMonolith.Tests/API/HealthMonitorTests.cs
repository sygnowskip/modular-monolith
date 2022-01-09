using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.API
{
    [TestFixture]
    public class HealthMonitorTests : BaseScenariosTests
    {
        [Test]
        public async Task ShouldHealthMonitorReturns200()
        {
            var httpClient = await HttpClientProvider.PrepareClientWithTokenForScopesAsync("HealthMonitor");
            var result = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, "api/health-monitor"));

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}