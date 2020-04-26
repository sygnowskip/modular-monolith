using System;
using System.Threading;
using MassTransit;
using MassTransit.Util;
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

        public void Run()
        {
            Console.WriteLine($"{DateTime.UtcNow} Started...");
            TaskUtil.Await(() => _busControl.StartAsync(CancellationToken.None));
        }
    }
}