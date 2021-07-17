using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExternalSystem.Events.Subjects;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.QueryServices;
using ModularMonolith.QueryServices.Common;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    public class SubjectsTests : BaseHttpTests
    {
        [Test]
        public async Task ShouldGetListOfSubjectsAsync()
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            var results = await GetSubjectsAsync(httpClient);
            results.Any().Should().BeTrue();
        }

        [Test]
        public async Task ShouldSynchroniseSubjectsAfterAdd()
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            var bus = ServiceProvider.GetRequiredService<IBusControl>();
            var response = await GetSubjectsAsync(httpClient);
            var currentCount = response.Count();

            await bus.Publish(new SubjectAdded(6, "Economy", DateTime.UtcNow));

            var result = await TryUntilSuccess(async () =>
            {
                var results = await GetSubjectsAsync(httpClient);
                return results.Count() == currentCount + 1;
            }, 5, TimeSpan.FromSeconds(3));

            result.Should().BeTrue();
        }

        [Test]
        public async Task ShouldSynchroniseSubjectsAfterDelete()
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            var bus = ServiceProvider.GetRequiredService<IBusControl>();
            var response = await GetSubjectsAsync(httpClient);
            var currentCount = response.Count();

            await bus.Publish(new SubjectDeleted(6, DateTime.UtcNow));

            var result = await TryUntilSuccess(async () =>
            {
                var results = await GetSubjectsAsync(httpClient);
                return results.Count() == currentCount - 1;
            }, 5, TimeSpan.FromSeconds(3));

            result.Should().BeTrue();
        }

        private async Task<IEnumerable<SubjectDto>> GetSubjectsAsync(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, "/api/subjects"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            return await DeserializeAsync<IEnumerable<SubjectDto>>(response);
        }
    }
}