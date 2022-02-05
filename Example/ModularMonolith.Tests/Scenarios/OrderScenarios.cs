using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Contracts.Orders;
using ModularMonolith.Tests.Common;
using Newtonsoft.Json;

namespace ModularMonolith.Tests.Scenarios
{
    public class OrderScenarios
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly MonolithApiSettings _monolithApiSettings;

        public OrderScenarios(IHttpClientProvider httpClientProvider, MonolithApiSettings monolithApiSettings)
        {
            _httpClientProvider = httpClientProvider;
            _monolithApiSettings = monolithApiSettings;
        }

        public OrderScenarios Given() => this;
        public OrderScenarios When() => this;
        public OrderScenarios Then() => this;
        public OrderScenarios And() => this;

        public async Task<OrderDto> OrderShouldBeCreatedAsync(long orderId)
        {
            var httpClient = await PrepareClientAsync();
            var orderResult = await httpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl, $"/api/orders/{orderId}"));

            orderResult.IsSuccessStatusCode.Should().BeTrue();
            
            return JsonConvert.DeserializeObject<OrderDto>(await orderResult.Content.ReadAsStringAsync());
        }

        private Task<HttpClient> PrepareClientAsync() =>
            _httpClientProvider.PrepareClientWithTokenForScopesAsync(nameof(OrderScenarios));
    }
}