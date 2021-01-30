using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Hexure.Events.Serialization;
using Hexure.Results;
using Hexure.Results.Extensions;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Hexure.EventsPublisher
{
    public class EventsPublisher
    {
        private readonly EventsPublisherSettings _settings;
        private readonly IBusControl _busControl;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly IServiceProvider _serviceProvider;

        public EventsPublisher(EventsPublisherSettings settings,
            IBusControl busControl,
            IEventDeserializer eventDeserializer,
            IServiceProvider serviceProvider)
        {
            _settings = settings;
            _busControl = busControl;
            _eventDeserializer = eventDeserializer;
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            await _busControl.StartAsync(stoppingToken);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var allEventsPublished = false;
                        while (!allEventsPublished)
                        {
                            allEventsPublished = await PublishAsync(stoppingToken);
                        }
                    }
                    catch (Exception ex) when (ex is SqlException || ex is InvalidOperationException)
                    {
                        Console.WriteLine(ex.ToString());
                        //TODO: Logging
                    }
                    finally
                    {
                        await Task.Delay(_settings.Delay, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _busControl.StopAsync(stoppingToken);
            }
        }

        private async Task<bool> PublishAsync(CancellationToken stoppingToken)
        {
            using var scopedContainer = _serviceProvider.CreateScope();
            var transactionProvider =
                scopedContainer.ServiceProvider.GetRequiredService<ITransactionProvider>();
            var serializedEventRepository = scopedContainer.ServiceProvider
                .GetRequiredService<ISerializedEventRepository>();

            await transactionProvider.BeginTransactionAsync();
            Console.WriteLine(
                $"{DateTime.UtcNow} Publishing (batch size: {_settings.BatchSize})...");
            var eventsToPublish =
                await serializedEventRepository.GetUnpublishedEventsAsync(_settings.BatchSize);

            var publishActions = eventsToPublish
                .Select(serializedEventEntity => _eventDeserializer.Deserialize(serializedEventEntity.SerializedEvent)
                    .OnSuccess(@event => _busControl.Publish(@event, stoppingToken)));

            Result.Combine(await Task.WhenAll(publishActions))
                .OnSuccess(() => serializedEventRepository.RemoveRange(eventsToPublish))
                .OnFailure(errors => throw new InvalidOperationException(JsonConvert.SerializeObject(errors)));

            await transactionProvider.CommitTransactionAsync();

            Console.WriteLine(
                $"{DateTime.UtcNow} Publish completed (events: {eventsToPublish.Count}, batch size: {_settings.BatchSize})...");

            return eventsToPublish.Count < _settings.BatchSize;
        }
    }
}