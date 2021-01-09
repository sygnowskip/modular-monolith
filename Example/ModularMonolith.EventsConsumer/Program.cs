using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
                    webBuilder.UseStartup<EventsConsumerStartup>();
                });
    }
}