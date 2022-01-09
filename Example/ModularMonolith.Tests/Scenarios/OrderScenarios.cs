using System;
using System.Net.Http;
using System.Threading.Tasks;
using ModularMonolith.Tests.Common;

namespace ModularMonolith.Tests.Scenarios
{
    public class OrderScenarios
    {
        private readonly IHttpClientProvider _httpClientProvider;

        public OrderScenarios(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public OrderScenarios Given() => this;
        public OrderScenarios When() => this;
        public OrderScenarios Then() => this;
        public OrderScenarios And() => this;

        public Task OrderShouldBeCreatedAsync()
        {
            throw new NotImplementedException();
        }

        private Task<HttpClient> PrepareClientAsync() =>
            _httpClientProvider.PrepareClientWithTokenForScopesAsync(nameof(OrderScenarios));
    }
}