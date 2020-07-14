using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.EventsConsumer
{
    public class EventsConsumer
    {
        private readonly IBusControl _busControl;

        public EventsConsumer(IServiceProvider serviceProvider)
        {
            _busControl = serviceProvider.GetRequiredService<IBusControl>();
        }

        public async Task RunAsync()
        {
            await _busControl.StartAsync();
            try
            {
                Console.WriteLine($"{DateTime.UtcNow} Started...");
                Console.WriteLine("Press enter to exit");

                await Task.Run(Console.ReadLine);
            }
            finally
            {
                await _busControl.StopAsync();
            }
        }
    }
}