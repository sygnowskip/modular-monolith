using Hexure.Workers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModularMonolith.Configuration;

namespace ModularMonolith.EventsConsumer
{
    public static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var httpBindings = ApplicationSettingsConfigurationProvider.Get()
                        .GetSection(nameof(HttpBindings))
                        .Get<HttpBindings>();
                    
                    webBuilder.UseUrls(httpBindings.Values);
                    webBuilder.UseStartup<EventsConsumerStartup>();
                });
    }
}