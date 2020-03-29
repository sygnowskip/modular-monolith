using System.IO;
using Microsoft.Extensions.Configuration;

namespace ModularMonolith.Configuration
{
    public static class ApplicationSettingsConfigurationProvider
    {
        public static IConfigurationRoot Get(string filename = "appsettings.json")
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(filename, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}