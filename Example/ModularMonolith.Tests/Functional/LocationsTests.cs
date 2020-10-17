using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExternalSystem.Events.Locations;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.QueryServices;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    public class LocationsTests : BaseHttpTests
    {
        [Test]
        public async Task ShouldGetListOfSubjectsAsync()
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            var results = await GetLocationsAsync(httpClient);
            results.Any().Should().BeTrue();
        }

        [Test]
        public async Task ShouldSynchroniseLocationAfterAdd()
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            var bus = ServiceProvider.GetRequiredService<IBusControl>();
            var response = await GetLocationsAsync(httpClient);
            var currentCount = response.Count();

            await bus.Publish(new LocationAdded(6, "Moscow", DateTime.UtcNow));

            var result = await TryUntilSuccess(async () =>
            {
                var results = await GetLocationsAsync(httpClient);
                return results.Count() == currentCount + 1;
            }, 5, TimeSpan.FromSeconds(3));

            result.Should().BeTrue();
        }

        [Test]
        public async Task ShouldSynchroniseLocationAfterDelete()
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            var bus = ServiceProvider.GetRequiredService<IBusControl>();
            var response = await GetLocationsAsync(httpClient);
            var currentCount = response.Count();
            
            await bus.Publish(new LocationDeleted(6, DateTime.UtcNow));

            var result = await TryUntilSuccess(async () =>
            {
                var results = await GetLocationsAsync(httpClient);
                return results.Count() == currentCount - 1;
            }, 5, TimeSpan.FromSeconds(3));

            result.Should().BeTrue();
        }

        private async Task<IEnumerable<LocationDto>> GetLocationsAsync(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, "/api/locations"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            return await DeserializeAsync<IEnumerable<LocationDto>>(response);
        }
    }
}