using System;
using System.Threading.Tasks;

namespace Hexure.EventsPublisher
{
    public class EventsPublisher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly int _defaultBatchSize;
        private readonly TimeSpan _defaultDelay;

        public EventsPublisher(int defaultBatchSize, TimeSpan defaultDelay, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _defaultBatchSize = defaultBatchSize;
            _defaultDelay = defaultDelay;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine($"{DateTime.UtcNow} Publishing (batch size: {_defaultBatchSize})...");
                await Task.Delay(_defaultDelay);
            }
        }
    }
}