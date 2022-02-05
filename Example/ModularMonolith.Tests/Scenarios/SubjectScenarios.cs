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
    public class SubjectScenarios
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly MonolithApiSettings _monolithApiSettings;
        
        public SubjectScenarios(IHttpClientProvider httpClientProvider, MonolithApiSettings monolithApiSettings)
        {
            _httpClientProvider = httpClientProvider;
            _monolithApiSettings = monolithApiSettings;
        }

        public SubjectScenarios Given() => this;
        public SubjectScenarios When() => this;
        public SubjectScenarios Then() => this;
        
        public void HaveSubjectsInDatabase() {}
        
        public async Task<IEnumerable<SubjectDto>> GetSubjectsAsync()
        {
            var httpClient = await PrepareClientAsync();
            var response = await httpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl, "/api/subjects"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            return await _httpClientProvider.DeserializeAsync<IEnumerable<SubjectDto>>(response);
        }

        public async Task SubjectsCountShouldBeEqualToAsync(int expectedSubjectsCount)
        {
            var result = await Try.UntilSuccess(async () =>
            {
                var results = await GetSubjectsAsync();
                return results.Count() == expectedSubjectsCount;
            }, 5, TimeSpan.FromSeconds(3));

            result.Should().BeTrue();
        }

        private Task<HttpClient> PrepareClientAsync() =>
            _httpClientProvider.PrepareClientWithTokenForScopesAsync(nameof(SubjectScenarios));
    }
}