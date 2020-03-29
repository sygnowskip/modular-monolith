using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.API
{
    [TestFixture]
    public class HealthMonitorTests : BaseHttpTests
    {
        [Test]
        public async Task ShouldHealthMonitorReturns200()
        {
            var result = await HttpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, "api/healthmonitor"));

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}