using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Contracts.Registrations;
using ModularMonolith.Tests.Common;
using Newtonsoft.Json;

namespace ModularMonolith.Tests.Scenarios
{
    public class RegistrationScenarios
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly MonolithApiSettings _monolithApiSettings;

        public RegistrationScenarios(IHttpClientProvider httpClientProvider, MonolithApiSettings monolithApiSettings)
        {
            _httpClientProvider = httpClientProvider;
            _monolithApiSettings = monolithApiSettings;
        }

        public RegistrationScenarios Given() => this;
        public RegistrationScenarios When() => this;
        public RegistrationScenarios Then() => this;
        public RegistrationScenarios And() => this;

        public async Task<GetSingleRegistrationDto> CreateRegistrationAsync(long examId)
        {
            var httpClient = await PrepareClientAsync();
            var creationRequestContent = _httpClientProvider.Serialize(new CreateRegistrationRequest("John", "Smith",
                new DateTime(1980, 03, 01), examId,
                new CreateRegistrationRequestInvoiceData("John Smith", "Street 1/2", "Warsaw", "00-999")));
            var creationResult = await httpClient.PostAsync(new Uri(_monolithApiSettings.BaseUrl, "/api/registrations"),
                creationRequestContent);
            creationResult.StatusCode.Should().Be(HttpStatusCode.Created);

            return await GetRegistrationAsync(creationResult.Headers.Location);
        }
        
        private async Task<GetSingleRegistrationDto> GetRegistrationAsync(Uri resourceLocation)
        {
            var httpClient = await PrepareClientAsync();
            var getResult = await httpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl,
                resourceLocation));
            getResult.IsSuccessStatusCode.Should().Be(true);

            return JsonConvert.DeserializeObject<GetSingleRegistrationDto>(await getResult.Content.ReadAsStringAsync());
        }

        private Task<HttpClient> PrepareClientAsync() =>
            _httpClientProvider.PrepareClientWithTokenForScopesAsync(nameof(RegistrationScenarios));
    }
}