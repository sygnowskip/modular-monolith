using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
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
            var response = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, "/api/locations"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var results = await DeserializeAsync<IEnumerable<LocationDto>>(response);
            results.Any().Should().BeTrue();
        }
    }
}