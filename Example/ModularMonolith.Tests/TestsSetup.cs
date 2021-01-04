using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hexure.Testing.Docker;
using Hexure.Testing.Docker.Common;
using Hexure.Testing.Resources;
using Hexure.Testing.Snapshots;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.None)]

namespace ModularMonolith.Tests
{
    [SetUpFixture]
    public class TestsSetup
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Snapshot _snapshot;
        private readonly DockerSetup _dockerSetup;

        public TestsSetup()
        {
            _serviceProvider = ServiceProviderBuilder.Build();
            _snapshot = _serviceProvider.GetRequiredService<Snapshot>();

            _dockerSetup = DockerSetup.Create(SolutionPathProvider.GetSolutionPath("Hexure").DirectoryName,
                new List<string>()
                {
                    "docker-compose.yml"
                });
        }

        [OneTimeSetUp]
        public async Task SetUp()
        {
#if RELEASE
            _dockerSetup.Up();
#endif
            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            var authoritySettings = _serviceProvider.GetRequiredService<AuthoritySettings>();
            var monolithApiSettings = _serviceProvider.GetRequiredService<MonolithApiSettings>();
            var msSqlAwaiter = _serviceProvider.GetRequiredService<MsSqlResourceAwaiter>();
            var rabbitMqAwaiter = _serviceProvider.GetRequiredService<RabbitMqResourceAwaiter>();
            var identityServerAwaiter = _serviceProvider.GetRequiredService<IdentityServerAwaiter>();
            var webApiAwaiter = _serviceProvider.GetRequiredService<WebApiResourceAwaiter>();



            Task.WaitAll(
                msSqlAwaiter.WaitForConnectionAsync(
                    configuration.GetConnectionString(ApplicationSettings.ConnectionStrings.Database)),
                rabbitMqAwaiter.WaitForConnectionAsync(),
                identityServerAwaiter.WaitForScopeAsync(authoritySettings.Url, authoritySettings.RequiredScope),
                webApiAwaiter.WaitForEndpointAsync(monolithApiSettings.Url + "api/health-monitor")
            );

            await _snapshot.DeleteAsync();
            await _snapshot.CreateAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
#if RELEASE
            _dockerSetup.Dispose();
#endif
            await _snapshot.RestoreAsync();
            await _snapshot.DeleteAsync();
        }
    }
}