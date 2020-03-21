
using System.Collections.Generic;
using Hexure.Testing.Docker;
using Hexure.Testing.Docker.Common;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.None)]

namespace ModularMonolith.Tests
{
    [SetUpFixture]
    public class TestsSetup
    {
        private readonly DockerSetup _dockerSetup;

        public TestsSetup()
        {
            _dockerSetup = DockerSetup.Create(SolutionPathProvider.GetSolutionPath("Hexure").DirectoryName,
                new List<string>()
                {
                    "docker-compose.yml"
                });
        }

        [OneTimeSetUp]
        public void SetUp()
        {
#if RELEASE
            _dockerSetup.Up();
#endif
        }

        [OneTimeTearDown]
        public void TearDown()
        {
#if RELEASE
            _dockerSetup.Dispose();
#endif
        }
    }
}