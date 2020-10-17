using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Hexure.EventsPublisher
{
    public class EventsPublisherBackgroundService : BackgroundService
    {
        private readonly EventsPublisher _eventsPublisher;

        public EventsPublisherBackgroundService(EventsPublisher eventsPublisher)
        {
            _eventsPublisher = eventsPublisher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var publisherTask = _eventsPublisher.RunAsync(stoppingToken);

            return publisherTask.IsCompleted
                ? publisherTask
                : Task.CompletedTask;
        }
    }
}