using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.QueryServices.Common;
using ModularMonolith.Tests.Common;

namespace ModularMonolith.Tests.Scenarios
{
    public class LocationScenarios
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly MonolithApiSettings _monolithApiSettings;

        public LocationScenarios(IHttpClientProvider httpClientProvider, MonolithApiSettings monolithApiSettings)
        {
            _httpClientProvider = httpClientProvider;
            _monolithApiSettings = monolithApiSettings;
        }

        public LocationScenarios Given() => this;
        public LocationScenarios When() => this;
        public LocationScenarios Then() => this;
        
        public void HaveLocationsInDatabase() {}
        
        public async Task<IEnumerable<LocationDto>> GetLocationsAsync()
        {
            var httpClient = await PrepareClientAsync();
            var response = await httpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl, "/api/locations"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            return await _httpClientProvider.DeserializeAsync<IEnumerable<LocationDto>>(response);
        }

        public async Task LocationCountShouldEqualToAsync(int expectedLocationsCount)
        {
            var result = await Try.UntilSuccess(async () =>
            {
                var results = await GetLocationsAsync();
                return results.Count() == expectedLocationsCount;
            }, 5, TimeSpan.FromSeconds(3));

            result.Should().BeTrue();
        }

        private Task<HttpClient> PrepareClientAsync() =>
            _httpClientProvider.PrepareClientWithTokenForScopesAsync(nameof(LocationScenarios));
    }
}