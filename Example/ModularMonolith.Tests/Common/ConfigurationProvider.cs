using System.IO;
using Microsoft.Extensions.Configuration;

namespace ModularMonolith.Tests.Common
{
    public class ApplicationSettingsConfigurationProvider
    {
        private ApplicationSettingsConfigurationProvider() { }

        public static IConfigurationRoot Get()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}