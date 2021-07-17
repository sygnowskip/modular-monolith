using System;
using System.Net.Http;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;

namespace ModularMonolith.Tests.Performance.Scenarios
{
    public static class DapperConnectionPoolingScenarioFactory
    {
        private static readonly TimeSpan WarmUpDuration = TimeSpan.FromSeconds(5);

        private static LoadSimulation[] BuildLoadSimulations() => new[]
        {
            Simulation.RampPerSec(rate: 100, during: TimeSpan.FromMinutes(3))
        };

        private static IStep BuildStep()
        {
            return Step.Create("Single", HttpClientFactory.Create(), async context =>
            {
                var request = Http.CreateRequest(HttpMethod.Get.Method, "https://localhost:5002/api/exams");
                return await Http.Send(request, context);
            });
        }

        public static Scenario Build()
        {
            return ScenarioBuilder
                .CreateScenario("Dapper connection pooling tests", BuildStep())
                .WithWarmUpDuration(WarmUpDuration)
                .WithLoadSimulations(BuildLoadSimulations());
        }
    }
}