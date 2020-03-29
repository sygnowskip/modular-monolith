using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using ModularMonolith.Configuration;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.API
{
    [TestFixture]
    public class HealthMonitorTests : BaseHttpTests
    {
        private readonly MonolithApiSettings _settings;

        public HealthMonitorTests()
        {
            _settings = ApplicationSettingsConfigurationProvider.Get().GetSection("Monolith").Get<MonolithApiSettings>();
        }

        [Test]
        public async Task ShouldHealthMonitorReturns200()
        {
            var result = await HttpClient.GetAsync(new Uri(_settings.BaseUrl, "api/healthmonitor"));

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}