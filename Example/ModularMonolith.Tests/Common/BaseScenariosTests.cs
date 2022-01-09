using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ModularMonolith.Tests.Common
{
    public abstract class BaseScenariosTests
    {
        protected readonly IServiceProvider ServiceProvider;
        private readonly IBusControl _busControl;
        protected readonly IHttpClientProvider HttpClientProvider;

        protected readonly MonolithApiSettings MonolithSettings;
        protected readonly Scenarios.Scenarios Scenarios;

        protected BaseScenariosTests()
        {
            ServiceProvider = ServiceProviderBuilder.Build();
            _busControl = ServiceProvider.GetRequiredService<IBusControl>();
            MonolithSettings = ServiceProvider.GetRequiredService<MonolithApiSettings>();
            HttpClientProvider = ServiceProvider.GetRequiredService<HttpClientProvider>();
            Scenarios = ServiceProvider.GetRequiredService<Scenarios.Scenarios>();
        }

        [OneTimeSetUp]
        public async Task SetUp()
        {
            await _busControl.StartAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _busControl.StopAsync();
        }
    }
}