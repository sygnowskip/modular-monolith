using ModularMonolith.Tests.Performance.Scenarios;
using NBomber.Configuration;
using NBomber.CSharp;

namespace ModularMonolith.Tests.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            NBomberRunner
                .RegisterScenarios(DapperConnectionPoolingScenarioFactory.Build())
                .WithTestName("ModularMonolith - Performance tests")
                .WithReportFormats(ReportFormat.Html)
                .Run();
        }
    }
}